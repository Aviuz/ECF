using ECF.BaseKit.CommandBase.Binding;
using System.Text;

namespace ECF;

/// <summary>
/// Command Flag
/// 
/// Register case-sensitive flag users can, but do not have to specify. Flags do not contain value. For example -p (short version), --myFlag (long version)
/// 
/// Note: If used on class that not implement BaseCommand it has no effects
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
[Obsolete("Please use ArgumentAttribute on property instead")]
public class CmdArgumentAttribute : Attribute
{
    public string Key { get; set; }

    public int Index { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    /// <summary>
    /// If set it will ignore arguments that start with any of the prefixes. It defaults to ["-"]
    /// </summary>
    public string[]? IgnorePrefixes { get; set; } = new string[] { "-" };

    public CmdArgumentAttribute(string key, int index)
    {
        Key = key;
        Index = index;
    }
}

[Obsolete("This is parser for obsolete attribute binding")]
internal class CommandArgument : ICommandBaseBinder
{
    private readonly CmdArgumentAttribute attribute;

    public CommandArgument(CmdArgumentAttribute attribute)
    {
        this.attribute = attribute;
    }

    public MatchingOrder GetMatchOrder() => MatchingOrder.Arguments_Obsolete;

    public bool TryMatch(ArgumentIterator visitor)
    {
        string? token = visitor.Get();

        if (token == null)
            return false;

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
        valueDictionary.StringValues[attribute.Key] = visitor.Get();
        visitor.AdvanceWithArgument();
    }

    public void AppendHelp(StringBuilder sb)
    {
        if (!string.IsNullOrWhiteSpace(attribute.Name))
            sb.Append($"\n\t{attribute.Index}: <{attribute.Name}>");
        else
            sb.Append($"\n\t{attribute.Index}: <value>");

        if (!string.IsNullOrWhiteSpace(attribute.Description))
            sb.Append($"\t {attribute.Description}");
    }

    public string SectionName() => "arguments";

    public int GetSyntaxOrder() => attribute.Index;

    public string GetSyntaxToken() => $"<{attribute.Name ?? attribute.Key}>";

    public void ThrowIfDefinitionContainsErrors() { }

    public bool ValidateAfterBinding(IList<string> errorMessages) => true;
}
