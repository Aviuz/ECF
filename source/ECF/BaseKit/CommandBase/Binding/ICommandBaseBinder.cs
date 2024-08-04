using System.Text;

namespace ECF.BaseKit.CommandBase.Binding;

internal interface ICommandBaseBinder
{
    // command execution
    bool TryMatch(ArgumentIterator visitor);
    void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary);
    MatchingOrder GetMatchOrder();

    // help generation
    void AppendHelp(StringBuilder sb);
    string SectionName();
    int GetSyntaxOrder();
    string? GetSyntaxToken();

    // other
    void ThrowIfDefinitionContainsErrors();
    bool ValidateAfterBinding(IList<string> errorMessages);
}
