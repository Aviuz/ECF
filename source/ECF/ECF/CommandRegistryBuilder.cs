using Autofac;
using ECF.Engine;
using ECF.Utilities;
using System.Reflection;

namespace ECF
{
    public class CommandRegistryBuilder
    {
        private readonly ContainerBuilder containerBuilder;
        private readonly CommandCollection collection;

        public CommandRegistryBuilder(ContainerBuilder containerBuilder, InterfaceContext interfaceContext)
        {
            this.containerBuilder = containerBuilder;

            collection = new CommandCollection();

            containerBuilder.RegisterInstance(interfaceContext);
            containerBuilder.RegisterType<CommandResolver>().As<ICommandResolver>().SingleInstance();
            containerBuilder.RegisterInstance(collection);
            containerBuilder.RegisterType<CommandDispatcher>();
        }

        public CommandRegistryBuilder RegisterCommands<TCommandAttribute>(params Assembly[] assemblies) where TCommandAttribute : Attribute, ICommandAttribute
        {
            foreach (Assembly assembly in assemblies)
            {
                foreach (var (type, attr) in CollectCommandTypes<TCommandAttribute>(assembly))
                {
                    collection.Register(attr, type);
                    containerBuilder.RegisterType(type).InstancePerDependency();
                }
            }

            return this;
        }

        private IEnumerable<(Type, ICommandAttribute)> CollectCommandTypes<TCommandAttribute>(Assembly assembly) where TCommandAttribute : Attribute, ICommandAttribute
        {
            foreach (var commandType in assembly.GetTypes())
            {
                var attr = commandType.GetCustomAttribute<TCommandAttribute>();

                if (attr != null)
                    yield return (commandType, attr);
            }
        }
    }
}
