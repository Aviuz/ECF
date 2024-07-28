using ECF.Exceptions;
using System.Text;

namespace ECF.CommandBaseComponents
{
    internal class CommandParameter : ICommandBaseParameter
    {
        private readonly CmdParameterAttribute attribute;

        public CommandParameter(CmdParameterAttribute attribute)
        {
            this.attribute = attribute;
        }

        public bool TryMatch(ArgumentIterator visitor)
        {
            string? token = visitor.Peek(0);

            return !string.IsNullOrWhiteSpace(attribute.LongName) && token == "--" + attribute.LongName
                || !string.IsNullOrWhiteSpace(attribute.ShortName) && token == "-" + attribute.ShortName;
        }

        public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
        {
            if (visitor.Peek(1) == null)
                throw new CommandBaseParseException($"You need you provide value after {visitor.Peek(0)} parameter flag");

            visitor.Take(true);
            valueDictionary.StringValues[attribute.Key] = visitor.Take(true);
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

        public int GetOrder() => int.MaxValue - 1;

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
    }
}
