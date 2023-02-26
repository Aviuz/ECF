using Autofac;
using ECF.Engine;
using ECF.Utilities;
using System.Reflection;

namespace ECF
{
    public static class CommandsConfiguration
    {
        public static ContainerBuilder RegisterECFBaseComponents(this ContainerBuilder builder, InterfaceContext context)
        {
            builder.RegisterInstance(context);
            builder.RegisterInstance(Context);
        }

        public static ContainerBuilder RegisterCommands<TCommandAttribute>(this ContainerBuilder builder, params Assembly[] assemblies) where TCommandAttribute : Attribute, ICommandAttribute
        {
            builder.RegisterType<CommandResolver<TCommandAttribute>>()
                .As<ICommandResolver>()
                .SingleInstance();

            HashSet<Assembly> assembliesSet = new();
            assembliesSet.Add(Assembly.GetExecutingAssembly());
            assembliesSet.Add(Assembly.GetCallingAssembly());
            assembliesSet.Add(Assembly.GetEntryAssembly()!);
            foreach (Assembly assembly in assemblies)
            {
                assembliesSet.Add(assembly);
            }

            CommandCollection<TCommandAttribute> collection = new(assembliesSet.ToArray());
            builder.RegisterInstance(collection);

            foreach (var (type, _) in collection.CollectCommandTypes())
            {
                builder.RegisterType(type).InstancePerDependency();
            }

            builder.RegisterType<CommandDispatcher>();

            return builder;
        }

        internal static ContainerBuilder UseContext(this ContainerBuilder builder, InterfaceContext context)
        {
            builder.RegisterInstance(context);
            return builder;
        }
    }
}
