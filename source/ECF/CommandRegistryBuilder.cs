using Autofac;
using ECF.Engine;
using ECF.Utilities;
using System.Reflection;

namespace ECF
{
    public class CommandRegistryBuilder
    {
        private readonly CommandCollection collection;

        public ContainerBuilder ContainerBuilder { get; }
        public InterfaceContext InterfaceContext { get; }

        public CommandRegistryBuilder(InterfaceContext? interfaceContext = null, ContainerBuilder? containerBuilder = null)
        {
            if (interfaceContext != null)
                InterfaceContext = interfaceContext;
            else
                InterfaceContext = new();

            if (containerBuilder != null)
                ContainerBuilder = containerBuilder;
            else
                ContainerBuilder = new();

            collection = new CommandCollection();

            ContainerBuilder.RegisterInstance(InterfaceContext);
            ContainerBuilder.RegisterType<CommandResolver>().As<ICommandResolver>().SingleInstance();
            ContainerBuilder.RegisterInstance(collection);
            ContainerBuilder.RegisterType<CommandDispatcher>();
        }

        public CommandRegistryBuilder RegisterCommands<TCommandAttribute>(params Assembly[] assemblies) where TCommandAttribute : Attribute, ICommandAttribute
        {
            foreach (Assembly assembly in assemblies)
            {
                foreach (var (type, attr) in CollectCommandTypes<TCommandAttribute>(assembly))
                {
                    collection.Register(attr, type);
                    ContainerBuilder.RegisterType(type).InstancePerDependency();
                }
            }

            return this;
        }

        public CommandRegistryBuilder Register<TCommandAttribute>(Type type) where TCommandAttribute : Attribute, ICommandAttribute
        {
            var attribute = type.GetCustomAttribute<TCommandAttribute>();

            if (attribute == null)
                throw new ECF.Exceptions.ECFException($"Type {type.FullName} doesn't have attribute named {typeof(TCommandAttribute).FullName}");

            collection.Register(attribute, type);
            ContainerBuilder.RegisterType(type).InstancePerDependency();

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
