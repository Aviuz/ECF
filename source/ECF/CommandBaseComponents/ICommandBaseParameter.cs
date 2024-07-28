using System.Text;

namespace ECF.CommandBaseComponents
{
    internal interface ICommandBaseParameter
    {
        // command execution
        bool TryMatch(ArgumentIterator visitor);
        void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary);

        // help generation
        void AppendHelp(StringBuilder sb);
        string SectionName();
        int GetOrder();
        string? GetSyntaxToken();
    }
}
