using ECF.Exceptions;
using ECF.Utilities;
using System.Reflection;
using System.Text;

namespace ECF.CommandBaseComponents
{
    internal class PropertyParameter : ICommandBaseParameter
    {
        private readonly CommandBase command;
        private readonly PropertyInfo propertyInfo;
        private readonly ParameterAttribute attribute;

        public PropertyParameter(CommandBase command, PropertyInfo propertyInfo, ParameterAttribute attribute)
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
            if (visitor.Peek(1) == null)
                throw new CommandBaseParseException($"You need you provide value after {visitor.Peek(0)} parameter flag");

            visitor.Take(true);
            string? value = visitor.Take(true);
            try
            {
                propertyInfo.SetValue(command, Convert.ChangeType(value, propertyInfo.PropertyType));
            }
            catch (FormatException)
            {
                throw new CommandBaseParseException($"Failed to cast value '{value}' to type {propertyInfo.PropertyType.Name}");
            }
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
    }
}
