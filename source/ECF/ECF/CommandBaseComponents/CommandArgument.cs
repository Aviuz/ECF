using System.Text;

namespace ECF.CommandBaseComponents
{
    internal class CommandArgument : ICommandBaseParameter
    {
        private readonly CmdArgumentAttribute attribute;

        public CommandArgument(CmdArgumentAttribute attribute)
        {
            this.attribute = attribute;
        }

        public bool TryMatch(ArgumentIterator visitor)
        {
            string? token = visitor.Peek(0);

            if (token == null) return false;

            if (token.StartsWith("-")) return false;

            if (visitor.CurrentOrderedIndex != attribute.Index)
                return false;

            return true;
        }

        public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
        {
            valueDictionary.StringValues[attribute.Key] = visitor.Take(false);
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
    }
}
