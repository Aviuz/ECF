using ECF.BaseKit.CommandBase.Binding;
using ECF.Exceptions;
using System.Reflection;
using System.Text;

namespace ECF;

/// <summary>
/// Command Flag
/// 
/// Register case-sensitive flag users can, but do not have to specify. Flags do not contain value. For example -p (short version), --myFlag (long version)
/// 
/// Note: If used on class that not implement BaseCommand it has no effects
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ArgumentAttribute : Attribute
{
    /// <summary>
    /// 0-based index of argument in command line. It will be used to bind value in specific order.
    /// </summary>
    public int Index { get; set; }

    /// <summary>
    /// Name that will be displayed in syntax help.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Description shown in help for this command.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// If set it will ignore arguments that start with any of the prefixes. It defaults to ["-"]
    /// </summary>
    public string[]? IgnorePrefixes { get; set; } = new string[] { "-" };

    public ArgumentAttribute(int index)
    {
        Index = index;
    }
}

internal class PropertyArgumentBinder : ICommandBaseBinder
{
    private readonly object parent;
    private readonly PropertyInfo propertyInfo;
    private readonly ArgumentAttribute attribute;
    private readonly RequiredAttribute? requiredAttr;

    private bool hasBeenSet = false;

    public PropertyArgumentBinder(object parent, PropertyInfo propertyInfo, ArgumentAttribute attribute)
    {
        this.parent = parent;
        this.propertyInfo = propertyInfo;
        this.attribute = attribute;

        requiredAttr = propertyInfo.GetCustomAttribute<RequiredAttribute>();
    }

    public MatchingOrder GetMatchOrder() => MatchingOrder.Arguments;

    public bool TryMatch(ArgumentIterator visitor)
    {
        string? token = visitor.Get();

        if (token == null) return false;

        if (attribute.IgnorePrefixes != null)
            foreach (var prefix in attribute.IgnorePrefixes)
                if (token.StartsWith(prefix))
                    return false;

        if (visitor.OrderedArgumentsConsumedCount != attribute.Index)
            return false;

        return true;
    }

    public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
    {
        propertyInfo.SetValue(parent, Convert.ChangeType(visitor.Get(), propertyInfo.PropertyType));
        hasBeenSet = true;
        visitor.AdvanceWithArgument();
    }

    public void AppendHelp(StringBuilder sb)
    {
        if (!string.IsNullOrWhiteSpace(attribute.Name))
            sb.Append($"\t\t{attribute.Index}: <{attribute.Name}>");
        else
            sb.Append($"\t\t{attribute.Index}: <value>");

        if (!string.IsNullOrWhiteSpace(attribute.Description))
            sb.Append($"\t {attribute.Description}");

        sb.AppendLine();
    }

    public string SectionName() => "Arguments";

    public int GetSyntaxOrder() => attribute.Index;

    public string GetSyntaxToken()
    {
        string token = string.IsNullOrEmpty(attribute.Name) == false
            ? $"<{attribute.Name}>"
            : $"<{attribute.Index}>";

        if (requiredAttr == null)
            token = $"[{token}]";

        return token;
    }

    public void ThrowIfDefinitionContainsErrors()
    {
        if (attribute.Index < 0)
            throw new CommandBaseParseException($"Argument {parent.GetType().FullName}.{propertyInfo.Name} has got negative index. Use 0-based index instead (0,1,2,3..)");
    }

    public bool ValidateAfterBinding(IList<string> errorMessages)
    {
        if (requiredAttr != null && hasBeenSet == false)
        {
            if (requiredAttr.ErrorMessage != null)
                errorMessages.Add(requiredAttr.ErrorMessage);
            else if (string.IsNullOrWhiteSpace(attribute.Name) == false)
                errorMessages.Add($"Argument <{attribute.Name}> is required");
            else
                errorMessages.Add($"Argument <{attribute.Index}> is required");

            return false;
        }

        return true;
    }
}
