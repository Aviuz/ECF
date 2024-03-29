﻿using System.Reflection;
using System.Text;

namespace ECF.CommandBaseComponents
{
    internal class PropertyArgument : ICommandBaseParameter
    {
        private readonly CommandBase command;
        private readonly PropertyInfo propertyInfo;
        private readonly ArgumentAttribute attribute;

        public PropertyArgument(CommandBase command, PropertyInfo propertyInfo, ArgumentAttribute attribute)
        {
            this.command = command;
            this.propertyInfo = propertyInfo;
            this.attribute = attribute;
        }

        public bool TryMatch(ArgumentIterator visitor)
        {
            string? token = visitor.Peek(0);

            if (token == null) return false;

            if (token.StartsWith("-")) return false;

            if (visitor.CurrentIndex != attribute.Index)
                return false;

            return true;
        }

        public void Apply(ArgumentIterator visitor, ValueDictionary valueDictionary)
        {
            propertyInfo.SetValue(command, Convert.ChangeType(visitor.Take(false), propertyInfo.PropertyType));
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
