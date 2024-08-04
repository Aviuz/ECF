using ECF.BaseKit.CommandBase.Binding;
using ECF.Exceptions;
using ECF.Utilities;
using System.Reflection;
using System.Text;

namespace ECF;

/// <summary>
/// Command Parameter
/// 
/// Register case-sensitive parameter with value that users can, but do not have to enter. For example -p <value> (short version), --myParam <value> (long version)
/// 
/// Note: If used on class that not implement BaseCommand it has no effects
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ParameterAttribute : Attribute
{
    /// <summary>
    /// Array of names that will be used to match parameter. It is case sensitive. For example ["-p", "--myParam"].
    /// </summary>
    public string[] Names { get; set; }

    /// <summary>
    /// Description shown in help for this command.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// String comaprison mode for matching names. Default is InvariantCulture.
    /// </summary>
    public StringComparison ComparisonMode { get; set; } = StringComparison.InvariantCulture;

    /// <summary>
    /// If set it will ignore value token (second one) if it starts with any of the prefixes, and eventually, it won't bind. It defaults to ["-"].
    /// </summary>
    public string[]? ForbiddenValuePrefixes { get; set; } = new string[] { "-" };

    [Obsolete("For same behaviour use Names (or names in constructor) instead with '-' prefix.")]
    public string? ShortName { get; set; }

    [Obsolete("For same behaviour use Names (or names in constructor) instead with '--' prefix.")]
    public string? LongName { get; set; }

    public ParameterAttribute(params string[] names)
    {
        Names = names ?? new string[0];
    }
}

internal class PropertyParameterBinder : ICommandBaseBinder
{
    private readonly object parent;
    private readonly PropertyInfo propertyInfo;
    private readonly ParameterAttribute attribute;
    private readonly RequiredAttribute? requiredAttr;

    private bool hasBeenSet = false;

    public PropertyParameterBinder(object parent, PropertyInfo propertyInfo, ParameterAttribute attribute)
    {
        this.parent = parent;
        this.propertyInfo = propertyInfo;
        this.attribute = attribute;

        requiredAttr = propertyInfo.GetCustomAttribute<RequiredAttribute>();
    }

    public MatchingOrder GetMatchOrder() => MatchingOrder.FlagsAndParameters;

    public bool TryMatch(ArgumentIterator visitor) =>
        MatchFirstToken(visitor.Get()!) && MatchSecondToken(visitor.Get(1));

    private bool MatchFirstToken(string token)
    {
        foreach (var name in GetFixedNames())
            if (token.Equals(name, attribute.ComparisonMode))
                return true;

        return false;
    }

    private bool MatchSecondToken(string? token)
    {
        if (token == null) return false;

        if (attribute.ForbiddenValuePrefixes != null)
            foreach (var prefix in attribute.ForbiddenValuePrefixes)
                if (token.StartsWith(prefix))
                    return false;

        return true;
    }

    public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
    {
        if (visitor.Get(1) == null)
            throw new CommandBaseParseException($"You need to provide value after {visitor.Get(0)} parameter flag");

        visitor.Advance();
        string? value = visitor.Get();
        try
        {
            propertyInfo.SetValue(parent, ExConverter.ChangeType(value, propertyInfo.PropertyType));
            hasBeenSet = true;
        }
        catch (FormatException)
        {
            throw new CommandBaseParseException($"Failed to cast value '{value}' to type {propertyInfo.PropertyType.Name}");
        }

        visitor.Advance();
    }

    public void AppendHelp(StringBuilder sb) =>
        sb.AppendLine($"\t\t{string.Join(", ", GetFixedNames())}\t {attribute.Description}");

    public string SectionName() => "Parameters";

    public int GetSyntaxOrder() => int.MaxValue - 1;

    public string? GetSyntaxToken() => requiredAttr != null
        ? $"{string.Join('|', GetFixedNames())} <value>"
        : $"[{string.Join('|', GetFixedNames())} <value>]";

#pragma warning disable CS0618 // Type or member is obsolete, Reason: backward compatibility
    private IEnumerable<string> GetFixedNames()
    {
        if (string.IsNullOrWhiteSpace(attribute.LongName) == false)
            yield return $"--{attribute.LongName}";
        if (string.IsNullOrWhiteSpace(attribute.ShortName) == false)
            yield return $"-{attribute.ShortName}";
        foreach (var name in attribute.Names.Where(x => string.IsNullOrWhiteSpace(x) == false))
            foreach (var splittedName in name.Split())
                yield return splittedName;
    }
#pragma warning restore CS0618 // Type or member is obsolete

#pragma warning disable CS0618 // Type or member is obsolete, Reason: backward compatibility
    public void ThrowIfDefinitionContainsErrors()
    {
        bool shouldThrow =
             string.IsNullOrWhiteSpace(attribute.ShortName)
             && string.IsNullOrWhiteSpace(attribute.LongName)
             && (
                attribute.Names.Length == 0
                || attribute.Names.All(x => string.IsNullOrWhiteSpace(x)));

        if (shouldThrow)
            throw new CommandBaseParseException($"Flag {parent.GetType().FullName}.{propertyInfo.Name} has no names defined.");
    }
#pragma warning restore CS0618 // Type or member is obsolete

    public bool ValidateAfterBinding(IList<string> errorMessages)
    {
        if (requiredAttr != null && hasBeenSet == false)
        {
            if (requiredAttr.ErrorMessage != null)
                errorMessages.Add(requiredAttr.ErrorMessage);
            else
                errorMessages.Add($"Parameter {attribute.Names[0]} is required");

            return false;
        }

        return true;
    }
}
