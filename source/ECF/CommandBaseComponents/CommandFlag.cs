﻿using System.Text;

namespace ECF.CommandBaseComponents
{
    internal class CommandFlag : ICommandBaseParameter
    {
        private readonly CmdFlagAttribute attribute;

        public CommandFlag(CmdFlagAttribute attribute)
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
            visitor.Take(true);
            valueDictionary.BoolValues[attribute.Key] = true;
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
    }
}
