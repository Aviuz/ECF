using System.Reflection;
using System.Text;

namespace ECF
{
    public abstract class CommandBase : ICommand, IHaveHelp
    {
        private string[]? commandArguments;
        private int argumentIterator;

        private readonly CmdArgAttribute[] cmdArgs;
        private readonly CmdParamAttribute[] cmdParams;
        private readonly CmdFlagAttribute[] cmdFlags;

        private Dictionary<string, string?> argsOrParamValues = new Dictionary<string, string?>();
        private Dictionary<string, bool> flagValues = new Dictionary<string, bool>();

        public CommandBase()
        {
            cmdArgs = GetType().GetCustomAttributes<CmdArgAttribute>().ToArray();
            cmdParams = GetType().GetCustomAttributes<CmdParamAttribute>().ToArray();
            cmdFlags = GetType().GetCustomAttributes<CmdFlagAttribute>().ToArray();

            foreach (var arg in cmdArgs)
            {
                if (argsOrParamValues.ContainsKey(arg.Key))
                    throw new Exception($"found duplicate key {arg.Key} in {GetType().Name} command");
                argsOrParamValues[arg.Key] = null;
            }
            foreach (var param in cmdParams)
            {
                if (argsOrParamValues.ContainsKey(param.Key))
                    throw new Exception($"found duplicate key {param.Key} in {GetType().Name} command");
                argsOrParamValues[param.Key] = null;
            }
            foreach (var flag in cmdFlags)
            {
                if (flagValues.ContainsKey(flag.Key))
                    throw new Exception($"found duplicate key {flag.Key} in {GetType().Name} command");
                flagValues[flag.Key] = false;
            }
        }

        public string? GetValue(string key) => argsOrParamValues[key];

        public T? GetValue<T>(string key)
        {
            string? userString = GetValue(key);

            if (userString == null)
                return default;

            return (T)Convert.ChangeType(userString, typeof(T));
        }

        public T GetValue<T>(string key, T defaultValue)
        {
            string? userString = GetValue(key);

            if (string.IsNullOrEmpty(userString))
                return defaultValue;

            return (T)Convert.ChangeType(userString, typeof(T));
        }

        public bool IsFlagActive(string key) => flagValues[key];

        public virtual void ApplyArguments(CommandArguments args)
        {
            commandArguments = args.Arguments;
            int currentArgumentIndex = 0;
            for (argumentIterator = 0; argumentIterator < commandArguments.Length; argumentIterator++)
            {
                var argument = commandArguments[argumentIterator];

                if (!TryApplyFlag(argument) && !TryApplyParameter(argument))
                {
                    ApplyArgument(argument, currentArgumentIndex++);
                }
            }
        }

        public abstract void Execute();

        public virtual string GetHelp()
        {
            var commandAttribute = GetType().GetCustomAttributes()
                .Select(attr => attr as ICommandAttribute)
                .Where(attr => attr != null)
                .Cast<ICommandAttribute>()
                .First();
            string usageParameters = GetSyntaxExpression();
            string description = GetDescription();

            var sb = new StringBuilder();

            sb.AppendLine($"{commandAttribute.Name} Command:");

            if (commandAttribute.Aliases.Length > 0)
                sb.AppendLine($"\tAliases: {string.Join(", ", commandAttribute.Aliases)}");

            if (!string.IsNullOrEmpty(usageParameters))
                sb.AppendLine($"\tUsage: {commandAttribute.Name} {usageParameters}");

            if (!string.IsNullOrEmpty(description))
                sb.AppendLine($"\tDescription: {description}");

            ArgumentsDescription(sb);
            ParamsDescription(sb);
            FlagsDescription(sb);

            return sb.ToString();
        }

        private string? TakeNextArgument()
        {
            if (commandArguments == null || commandArguments.Length <= ++argumentIterator)
                return null;

            return commandArguments[argumentIterator];
        }

        private void ApplyArgument(string value, int index)
        {
            var argument = cmdArgs.FirstOrDefault(a => a.Index == index);

            if (argument != null)
            {
                argsOrParamValues[argument.Key] = value;
            }
        }

        private bool TryApplyParameter(string input)
        {
            foreach (var param in cmdParams)
            {
                bool isApplied =
                    (!string.IsNullOrWhiteSpace(param.LongName) && input == "--" + param.LongName)
                    || (!string.IsNullOrWhiteSpace(param.ShortName) && input == "-" + param.ShortName);

                if (isApplied)
                {
                    argsOrParamValues[param.Key] = TakeNextArgument();
                    return true;
                }
            }
            return false;
        }

        private bool TryApplyFlag(string input)
        {
            foreach (var flag in cmdFlags)
            {
                bool isApplied =
                    (!string.IsNullOrWhiteSpace(flag.LongName) && input == "--" + flag.LongName)
                    || (!string.IsNullOrWhiteSpace(flag.ShortName) && input == "-" + flag.ShortName);

                if (isApplied)
                {
                    flagValues[flag.Key] = true;
                    return true;
                }
            }
            return false;
        }

        private string GetSyntaxExpression()
        {
            return GetType().GetCustomAttribute<CmdSyntaxAttribute>()?.SyntaxExpression ?? string.Empty;
        }

        private string GetDescription()
        {
            return GetType().GetCustomAttribute<CmdDescriptionAttribute>()?.Description ?? string.Empty;
        }

        private void ArgumentsDescription(StringBuilder sb)
        {
            if (cmdArgs.Length != 0)
            {
                sb.AppendLine($"\tArguments:");
                foreach (var argument in cmdArgs)
                {
                    sb.AppendLine($"\t\t<{argument.Name ?? argument.Index.ToString()}> : {argument.Description ?? string.Empty}");
                }
            }
        }

        private void FlagsDescription(StringBuilder sb)
        {
            if (cmdFlags.Length != 0)
            {
                sb.AppendLine($"\tFlags:");
                foreach (var flag in cmdFlags)
                {
                    if (!string.IsNullOrEmpty(flag.ShortName) && !string.IsNullOrEmpty(flag.LongName))
                    {
                        sb.AppendLine($"\t\t--{flag.LongName}, -{flag.ShortName} : {flag.Description ?? string.Empty}");
                    }
                    else if (!string.IsNullOrEmpty(flag.LongName))
                    {
                        sb.AppendLine($"\t\t--{flag.LongName} : {flag.Description ?? string.Empty}");
                    }
                    else if (!string.IsNullOrEmpty(flag.ShortName))
                    {
                        sb.AppendLine($"\t\t-{flag.ShortName} : {flag.Description ?? string.Empty}");
                    }
                }
            }
        }

        private void ParamsDescription(StringBuilder sb)
        {
            if (cmdParams.Length != 0)
            {
                sb.AppendLine($"\tParameters:");
                foreach (var paramater in cmdParams)
                {
                    if (!string.IsNullOrEmpty(paramater.ShortName) && !string.IsNullOrEmpty(paramater.LongName))
                    {
                        sb.AppendLine($"\t\t--{paramater.LongName} <value>, -{paramater.ShortName} <value> : {paramater.Description ?? string.Empty}");
                    }
                    else if (!string.IsNullOrEmpty(paramater.LongName))
                    {
                        sb.AppendLine($"\t\t--{paramater.LongName} <value> : {paramater.Description ?? string.Empty}");
                    }
                    else if (!string.IsNullOrEmpty(paramater.ShortName))
                    {
                        sb.AppendLine($"\t\t-{paramater.ShortName} <value> : {paramater.Description ?? string.Empty}");
                    }
                }
            }
        }
    }
}
