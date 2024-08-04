using ECF.BaseKit.CommandBase.Binding;
using System.Text;

namespace ECF;

/// <summary>
/// Command Argument
/// 
/// Register argument that users will enter in specific order. User cannot change order, or name arguments. For example <argument_1> <argument_2>
/// 
/// Note: If used on class that not implement BaseCommand it has no effects
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
[Obsolete("Please use FlagAttribute on property instead")]
public class CmdFlagAttribute : Attribute
{
    public string Key { get; set; }
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public string? Description { get; set; }

    public CmdFlagAttribute(string key)
    {
        Key = key;
    }
}

[Obsolete("This is parser for obsolete attribute binding")]
internal class CommandFlag : ICommandBaseBinder
{
    private readonly CmdFlagAttribute attribute;

    public CommandFlag(CmdFlagAttribute attribute)
    {
        this.attribute = attribute;
    }

    public MatchingOrder GetMatchOrder() => MatchingOrder.FlagsAndParameters_Obsolete;

    public bool TryMatch(ArgumentIterator visitor)
    {
        string? token = visitor.Get();

        return !string.IsNullOrWhiteSpace(attribute.LongName) && token == "--" + attribute.LongName
            || !string.IsNullOrWhiteSpace(attribute.ShortName) && token == "-" + attribute.ShortName;
    }

    public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
    {
        valueDictionary.BoolValues[attribute.Key] = true;
        visitor.Advance();
    }

    public void AppendHelp(StringBuilder sb)
    {
        if (!string.IsNullOrEmpty(attribute.ShortName) && !string.IsNullOrEmpty(attribute.LongName))
            sb.Append($"\t\t--{attribute.LongName}, -{attribute.ShortName}");
        else if (!string.IsNullOrEmpty(attribute.LongName))
            sb.Append($"\t\t--{attribute.LongName}");
        else if (!string.IsNullOrEmpty(attribute.ShortName))
            sb.Append($"\t\t-{attribute.ShortName}");

        if (!string.IsNullOrWhiteSpace(attribute.Description))
            sb.Append($"\t {attribute.Description}");

        sb.AppendLine();
    }

    public string SectionName() => "Flags";

    public int GetSyntaxOrder() => int.MaxValue;

    public string? GetSyntaxToken()
    {
        if (string.IsNullOrWhiteSpace(attribute.LongName) == false && string.IsNullOrWhiteSpace(attribute.ShortName) == false)
            return $"[-{attribute.ShortName}|--{attribute.LongName}]";
        else if (string.IsNullOrWhiteSpace(attribute.LongName) == false)
            return $"[--{attribute.LongName}]";
        else if (string.IsNullOrWhiteSpace(attribute.ShortName) == false)
            return $"[-{attribute.ShortName}]";
        else
            return null;
    }

    public void ThrowIfDefinitionContainsErrors() { }

    public bool ValidateAfterBinding(IList<string> errorMessages) => true;
}
