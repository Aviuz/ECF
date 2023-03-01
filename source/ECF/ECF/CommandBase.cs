using ECF.CommandBaseComponents;
using System.Reflection;
using System.Text;

namespace ECF
{
    public abstract class CommandBase : ICommand, IHaveHelp
    {
        private List<ICommandBaseParameter> parameters = new();
        private ValueDictionary values = new();

        public CommandBase()
        {
            foreach (var attr in GetType().GetCustomAttributes<CmdArgumentAttribute>())
                parameters.Add(new CommandArgument(attr));

            foreach (var attr in GetType().GetCustomAttributes<CmdParameterAttribute>())
                parameters.Add(new CommandParameter(attr));

            foreach (var attr in GetType().GetCustomAttributes<CmdFlagAttribute>())
                parameters.Add(new CommandFlag(attr));

            foreach (var prop in GetType().GetProperties())
            {
                var argAttr = prop.GetCustomAttribute<ArgumentAttribute>();
                var paramAttr = prop.GetCustomAttribute<ParameterAttribute>();
                var flagAttr = prop.GetCustomAttribute<FlagAttribute>();

                if (argAttr != null)
                    parameters.Add(new PropertyArgument(this, prop, argAttr));
                if (paramAttr != null)
                    parameters.Add(new PropertyParameter(this, prop, paramAttr));
                if (flagAttr != null)
                    parameters.Add(new PropertyFlag(this, prop, flagAttr));
            }
        }

        public virtual void ApplyArguments(CommandArguments args)
        {
            ArgumentIterator tokenIterator = new(args.Arguments);

            while (tokenIterator.Peek(0) != null)
            {
                int indexAtStart = tokenIterator.CurrentIndex;

                foreach (var parameter in parameters)
                {
                    if (parameter.TryMatch(tokenIterator))
                        parameter.Apply(tokenIterator, values);
                }

                if (indexAtStart == tokenIterator.CurrentIndex)
                {
                    bool isMismatchedFlag = tokenIterator.Peek(0)?.StartsWith("-") ?? false;
                    tokenIterator.Take(isMismatchedFlag == true);
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
            string parametersHelp = GetParametersHelp();

            var sb = new StringBuilder();

            sb.AppendLine($"{commandAttribute.Name} Command:");

            if (commandAttribute.Aliases?.Length > 0)
                sb.AppendLine($"\tAliases: {string.Join(", ", commandAttribute.Aliases)}");

            if (!string.IsNullOrEmpty(usageParameters))
                sb.AppendLine($"\tUsage: {commandAttribute.Name} {usageParameters}");

            if (!string.IsNullOrEmpty(description))
                sb.AppendLine($"\tDescription: {description}");

            if (!string.IsNullOrEmpty(parametersHelp))
                sb.AppendLine(parametersHelp);

            return sb.ToString();
        }

        public void DisplayHelp()
        {
            Console.WriteLine(GetHelp());
        }

        public string? GetValue(string key)
        {
            if (!values.StringValues.ContainsKey(key))
                return null;

            return values.StringValues[key];
        }

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

        public bool IsFlagActive(string key)
        {
            if (!values.BoolValues.ContainsKey(key))
                return false;

            return values.BoolValues[key];
        }

        private string GetSyntaxExpression()
        {
            return GetType().GetCustomAttribute<CmdSyntaxAttribute>()?.SyntaxExpression ?? string.Empty;
        }

        private string GetDescription()
        {
            return GetType().GetCustomAttribute<CmdDescriptionAttribute>()?.Description ?? string.Empty;
        }

        private string GetParametersHelp()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var group in parameters.GroupBy(x => x.SectionName()))
            {
                sb.AppendLine($"\t{group.Key}:");
                foreach (var param in group)
                {
                    param.AppendHelp(sb);
                }
            }

            return sb.ToString();
        }
    }
}