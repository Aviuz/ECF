using System.Reflection;

namespace ECF.Engine
{
    /// <summary>
    /// Static reflection repository for command types and their command strings
    /// </summary>
    public class CommandCollection<TCommandAttribute> where TCommandAttribute : Attribute, ICommandAttribute
    {
        private readonly Assembly[] assemblies;
        private HashSet<Type> commandTypes = new HashSet<Type>();
        private Dictionary<string, Type> commandBindings = new Dictionary<string, Type>();

        internal CommandCollection(Assembly[] assemblies)
        {
            this.assemblies = assemblies;

            foreach (var (commandType, attr) in CollectCommandTypes())
            {
                // add command to collection
                commandTypes.Add(commandType);

                // add bindings
                commandBindings.Add(attr.Name.ToUpper(), commandType);
                foreach (var alias in attr.Aliases)
                    commandBindings.Add(alias.ToUpper(), commandType);
            }
        }

        public Type GetCommand(string binding)
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
                yield return commandType.GetCustomAttribute<TCommandAttribute>();
            }
        }

        internal IEnumerable<(Type, ICommandAttribute)> CollectCommandTypes()
        {
            foreach(var assembly in assemblies)
            foreach (var commandType in assembly.GetTypes())
            {
                var attr = commandType.GetCustomAttribute<TCommandAttribute>();

                if (attr != null)
                    yield return (commandType, attr);
            }
        }
    }
}
