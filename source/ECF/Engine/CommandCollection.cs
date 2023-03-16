using System.Reflection;

namespace ECF.Engine
{
    /// <summary>
    /// Static reflection repository for command types and their command strings
    /// </summary>
    public class CommandCollection
    {
        private HashSet<Type> commandTypes = new HashSet<Type>();
        private Dictionary<string, Type> commandBindings = new Dictionary<string, Type>();

        public Type? GetCommand(string binding)
        {
            binding = binding.ToUpper();
            if (!commandBindings.ContainsKey(binding))
                return null;

            return commandBindings[binding];
        }

        public IEnumerable<ICommandAttribute> GetAllCommands()
        {
            foreach (var commandType in commandTypes)
            {
                yield return (ICommandAttribute)commandType.GetCustomAttributes().Where(x => typeof(ICommandAttribute).IsAssignableFrom(x.GetType())).First();
            }
        }

        public void Register(ICommandAttribute commandAttribute, Type commandType)
        {
            // add command to collection
            commandTypes.Add(commandType);

            // add bindings
            commandBindings[commandAttribute.Name.ToUpper()] = commandType;
            if (commandAttribute.Aliases != null)
                foreach (var alias in commandAttribute.Aliases)
                    commandBindings[alias.ToUpper()] = commandType;
        }
    }
}
