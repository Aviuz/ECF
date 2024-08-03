using ECF.BaseKit.CommandBase.Binding;
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
    public int Index { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
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

    public PropertyArgumentBinder(object parent, PropertyInfo propertyInfo, ArgumentAttribute attribute)
    {
        this.parent = parent;
        this.propertyInfo = propertyInfo;
        this.attribute = attribute;
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

    public string GetSyntaxToken() => $"<{attribute.Name}>";

    public void Validate() { }
}
