using ECF.BaseKit.CommandBase.Binding;
using ECF.Exceptions;
using System.Text;

namespace ECF;

/// <summary>
/// Command Parameter
/// 
/// Register case-sensitive parameter with value that users can, but do not have to enter. For example -p <value> (short version), --myParam <value> (long version)
/// 
/// Note: If used on class that not implement BaseCommand it has no effects
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
[Obsolete("Please use ParameterAttribute on property instead")]
public class CmdParameterAttribute : Attribute
{
    public string Key { get; set; }
    public string? ShortName { get; set; }
    public string? LongName { get; set; }
    public string? Description { get; set; }

    public CmdParameterAttribute(string key)
    {
        Key = key;
    }
}

[Obsolete("This is parser for obsolete attribute binding")]
internal class CommandParameter : ICommandBaseBinder
{
    private readonly CmdParameterAttribute attribute;

    public CommandParameter(CmdParameterAttribute attribute)
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
        if (visitor.Get(1) == null)
            throw new CommandBaseParseException($"You need to provide value after {visitor.Get()} parameter flag");

        visitor.Advance();

        valueDictionary.StringValues[attribute.Key] = visitor.Get();
        visitor.Advance();
    }

    public void AppendHelp(StringBuilder sb)
    {
        if (!string.IsNullOrEmpty(attribute.ShortName) && !string.IsNullOrEmpty(attribute.LongName))
            sb.Append($"\t\t--{attribute.LongName} <value>, -{attribute.ShortName} <value>");
        else if (!string.IsNullOrEmpty(attribute.LongName))
            sb.Append($"\t\t--{attribute.LongName} <value>");
        else if (!string.IsNullOrEmpty(attribute.ShortName))
            sb.Append($"\t\t-{attribute.ShortName} <value>");

        if (!string.IsNullOrWhiteSpace(attribute.Description))
            sb.Append($"\t {attribute.Description}");

        sb.AppendLine();
    }

    public string SectionName() => "Parameters";

    public int GetSyntaxOrder() => int.MaxValue - 1;

    public string? GetSyntaxToken()
    {
        if (string.IsNullOrWhiteSpace(attribute.LongName) == false && string.IsNullOrWhiteSpace(attribute.ShortName) == false)
            return $"[-{attribute.ShortName}|--{attribute.LongName} <value>]";
        else if (string.IsNullOrWhiteSpace(attribute.LongName) == false)
            return $"[--{attribute.LongName} <value>]";
        else if (string.IsNullOrWhiteSpace(attribute.ShortName) == false)
            return $"[-{attribute.ShortName} <value>]";
        else
            return null;
    }

    public void ThrowIfDefinitionContainsErrors() { }

    public bool ValidateAfterBinding(IList<string> errorMessages) => true;
}
