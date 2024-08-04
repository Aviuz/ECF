using ECF.BaseKit.CommandBase.Binding;
using ECF.BaseKit.CommandBase.Binding.AttributesBinders;
using ECF.Utilities;
using System.Reflection;
using System.Text;

namespace ECF;

public abstract class AsyncCommandBase : ICommand, IHaveHelp
{
    private List<ICommandBaseBinder> parameterBinders = new();
    private ValueDictionary values = new();

    private HelpBinder helpBinder = new();

    public AsyncCommandBase()
    {
#pragma warning disable CS0618 // Type or member is obsolete, Reason: backward compatibility
        foreach (var attr in GetType().GetCustomAttributes<CmdArgumentAttribute>())
            parameterBinders.Add(new CommandArgument(attr));

        foreach (var attr in GetType().GetCustomAttributes<CmdParameterAttribute>())
            parameterBinders.Add(new CommandParameter(attr));

        foreach (var attr in GetType().GetCustomAttributes<CmdFlagAttribute>())
            parameterBinders.Add(new CommandFlag(attr));
#pragma warning restore CS0618 // Type or member is obsolete

        foreach (var prop in GetType().GetProperties())
        {
            var argAttr = prop.GetCustomAttribute<ArgumentAttribute>();
            var paramAttr = prop.GetCustomAttribute<ParameterAttribute>();
            var flagAttr = prop.GetCustomAttribute<FlagAttribute>();

            if (argAttr != null)
                parameterBinders.Add(new PropertyArgumentBinder(this, prop, argAttr));
            if (paramAttr != null)
                parameterBinders.Add(new PropertyParameterBinder(this, prop, paramAttr));
            if (flagAttr != null)
                parameterBinders.Add(new PropertyFlagBinder(this, prop, flagAttr));
        }

        parameterBinders.Add(helpBinder);

        foreach (var parameter in parameterBinders)
        {
            parameter.ThrowIfDefinitionContainsErrors();
        }
    }

    public async Task ExecuteAsync(CommandArguments args, CancellationToken cancellationToken)
    {
        ApplyArguments(args);

        if (helpBinder.WasTriggered)
        {
            DisplayHelp();
        }
        else if (Validate(out var errorMessages) == false)
        {
            foreach (var message in errorMessages)
            {
                ColorConsole.WriteLine(message, ConsoleColor.Yellow);
            }

            Console.WriteLine();
            Console.WriteLine($"CommandSyntax: {GetSyntaxExpression()}");
        }
        else
        {
            await ExecuteAsync(cancellationToken);
        }
    }

    public abstract Task ExecuteAsync(CancellationToken cancellationToken);

    protected virtual void ApplyArguments(CommandArguments args)
    {
        ArgumentIterator tokenIterator = new(args.Arguments);
        ICommandBaseBinder[] binders = parameterBinders
            .OrderBy(x => x.GetMatchOrder())
            .ToArray();

        while (tokenIterator.Get() != null)
        {
            int indexAtStart = tokenIterator.CurrentIndex;

            bool tokenConsumed = false;

            for (int i = 0; i < binders.Length && !tokenConsumed; i++)
            {
                if (binders[i].TryMatch(tokenIterator))
                {
                    binders[i].Apply(tokenIterator, values);
                    tokenConsumed = true;
                }
            }

            if (!tokenConsumed)
            {
                tokenIterator.Advance();
            }
        }
    }

    protected virtual bool Validate(out IList<string> errorMessages)
    {
        bool isValid = true;
        errorMessages = new List<string>();

        foreach (var parameter in parameterBinders)
            isValid &= parameter.ValidateAfterBinding(errorMessages);

        return isValid;
    }

    public virtual string GetHelp()
    {
        ICommandAttribute commandAttribute = GetCommandAttribute();
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

    private ICommandAttribute GetCommandAttribute()
    {
        return GetType().GetCustomAttributes()
             .Select(attr => attr as ICommandAttribute)
             .Where(attr => attr != null)
             .Cast<ICommandAttribute>()
             .First();
    }

    internal string GetSyntaxExpression()
    {
        string? customSyntax = GetType().GetCustomAttribute<CmdSyntaxAttribute>()?.SyntaxExpression;
        if (customSyntax != null) return customSyntax; // for developer specified syntax

        var strParts = parameterBinders
            .OrderBy(x => x.GetSyntaxOrder())
            .Select(x => x.GetSyntaxToken())
            .Where(x => x != null);

        return string.Join(" ", strParts);
    }

    private string GetDescription()
    {
        return GetType().GetCustomAttribute<CmdDescriptionAttribute>()?.Description ?? string.Empty;
    }

    private string GetParametersHelp()
    {
        StringBuilder sb = new StringBuilder();

        foreach (var group in parameterBinders.Where(x => x.SectionName() != null).GroupBy(x => x.SectionName()))
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