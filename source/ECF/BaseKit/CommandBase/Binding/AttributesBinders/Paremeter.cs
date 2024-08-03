using ECF.BaseKit.CommandBase.Binding;
using ECF.Exceptions;
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
    public string[] Names { get; set; }
    public string? Description { get; set; }

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

    public PropertyParameterBinder(object parent, PropertyInfo propertyInfo, ParameterAttribute attribute)
    {
        this.parent = parent;
        this.propertyInfo = propertyInfo;
        this.attribute = attribute;
    }

    public MatchingOrder GetMatchOrder() => MatchingOrder.FlagsAndParameters;

    public bool TryMatch(ArgumentIterator visitor)
    {
        Validate();

        string? token = visitor.Get();

        foreach (var name in GetFixedNames())
            if (token == name)
                return true;

        return false;
    }

    public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
    {
        if (visitor.Get(1) == null)
            throw new CommandBaseParseException($"You need to provide value after {visitor.Get(0)} parameter flag");

        visitor.Advance();
        string? value = visitor.Get();
        try
        {
            propertyInfo.SetValue(parent, Convert.ChangeType(value, propertyInfo.PropertyType));
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

    public string? GetSyntaxToken() =>
        $"[{string.Join('|', GetFixedNames())} <value>]";

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
    public void Validate()
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
}
