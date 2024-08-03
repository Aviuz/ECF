using ECF.BaseKit.CommandBase.Binding;
using ECF.Exceptions;
using System.Reflection;
using System.Text;

namespace ECF;

/// <summary>
/// Command Argument
/// 
/// Register argument that users will enter in specific order. User cannot change order, or name arguments. For example <argument_1> <argument_2>
/// 
/// Note: If used on class that not implement BaseCommand it has no effects
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class FlagAttribute : Attribute
{
    public string[] Names { get; set; }
    public string? Description { get; set; }

    [Obsolete("For same behaviour use Names (or names in constructor) instead with '-' prefix.")]
    public string? ShortName { get; set; }

    [Obsolete("For same behaviour use Names (or names in constructor) instead with '--' prefix.")]
    public string? LongName { get; set; }

    public FlagAttribute(params string[] names)
    {
        Names = names ?? new string[0];
    }
}

internal class PropertyFlagBinder : ICommandBaseBinder
{
    private readonly object parent;
    private readonly PropertyInfo propertyInfo;
    private readonly FlagAttribute attribute;

    public PropertyFlagBinder(object parent, PropertyInfo propertyInfo, FlagAttribute attribute)
    {
        this.parent = parent;
        this.propertyInfo = propertyInfo;
        this.attribute = attribute;
    }

    public MatchingOrder GetMatchOrder() => MatchingOrder.FlagsAndParameters;

    public bool TryMatch(ArgumentIterator visitor)
    {
        string? token = visitor.Get();

        foreach (var name in GetFixedNames())
            if (token == name)
                return true;

        return false;
    }

    public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
    {
        if (propertyInfo.PropertyType != typeof(bool))
            throw new CommandBaseParseException($"Property {propertyInfo.Name} need to be of type bool to be treated as flag.");

        propertyInfo.SetValue(parent, true);

        visitor.Advance();
    }

    public void AppendHelp(StringBuilder sb) =>
        sb.AppendLine($"\t\t{string.Join(", ", GetFixedNames())}\t {attribute.Description}");

    public string SectionName() => "Flags";

    public int GetSyntaxOrder() => int.MaxValue;

    public string? GetSyntaxToken() => $"[{string.Join('|', GetFixedNames())}]";

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
             && (attribute.Names.Length == 0
                || attribute.Names.All(x => string.IsNullOrWhiteSpace(x)));

        if (shouldThrow)
            throw new CommandBaseParseException($"Flag {parent.GetType().FullName}.{propertyInfo.Name} has no names defined.");
    }
#pragma warning restore CS0618 // Type or member is obsolete
}