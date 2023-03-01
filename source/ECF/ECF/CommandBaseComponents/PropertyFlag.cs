using ECF.Exceptions;
using System.Reflection;
using System.Text;

namespace ECF.CommandBaseComponents
{
    internal class PropertyFlag : ICommandBaseParameter
    {
        private readonly CommandBase command;
        private readonly PropertyInfo propertyInfo;
        private readonly FlagAttribute attribute;

        public PropertyFlag(CommandBase command, PropertyInfo propertyInfo, FlagAttribute attribute)
        {
            this.command = command;
            this.propertyInfo = propertyInfo;
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

            if (propertyInfo.PropertyType != typeof(bool))
                throw new CommandBaseParseException($"Property {propertyInfo.Name} need to be of type bool to be treated as flag.");

            propertyInfo.SetValue(command, true);
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
