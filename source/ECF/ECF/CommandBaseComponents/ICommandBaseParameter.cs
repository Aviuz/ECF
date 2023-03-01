using System.Text;

namespace ECF.CommandBaseComponents
{
    internal interface ICommandBaseParameter
    {
        bool TryMatch(ArgumentIterator visitor);
        void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary);
        void AppendHelp(StringBuilder sb);
        string SectionName();
    }
}
