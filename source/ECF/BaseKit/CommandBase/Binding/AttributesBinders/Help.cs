using System.Text;

namespace ECF.BaseKit.CommandBase.Binding.AttributesBinders;

internal class HelpBinder : ICommandBaseBinder
{
    private static readonly string[] Names = new string[] { "--help", "-h" };

    public bool WasTriggered { get; private set; }

    public bool TryMatch(ArgumentIterator visitor)
    {
        foreach (var name in Names)
            if (name.Equals(visitor.Get(), StringComparison.InvariantCulture))
                return true;

        return false;
    }

    public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
    {
        WasTriggered = true;
        visitor.Advance();
    }

    public MatchingOrder GetMatchOrder() => MatchingOrder.BaseKitBinders;

    public void AppendHelp(StringBuilder sb) { }
    public int GetSyntaxOrder() => int.MaxValue;
    public string? GetSyntaxToken() => null;
    public string? SectionName() => null;
    public void ThrowIfDefinitionContainsErrors() { }
    public bool ValidateAfterBinding(IList<string> errorMessages) => true;
}
