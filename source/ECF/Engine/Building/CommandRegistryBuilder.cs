using ECF.InverseOfControl;
using ECF.Utilities;
using System.Reflection;

namespace ECF.Engine;

/// <summary>
/// This builder registers command in IoC and map them in commands collection for you.
/// </summary>
public class CommandRegistryBuilder
{
    private readonly ICommandCollection collection = new CommandCollection();

    private IIoCBuilderAdapter IoCBuilderAdapter;

    public CommandRegistryBuilder(IIoCBuilderAdapter iocBuilderAdapter)
    {
        IoCBuilderAdapter = iocBuilderAdapter;

        IoCBuilderAdapter.RegisterIoCProviderAdapter();
        IoCBuilderAdapter.RegisterSingleton(collection);
        IoCBuilderAdapter.RegisterScoped<ICommandResolver, CommandResolver>();
        IoCBuilderAdapter.RegisterScoped<CommandDispatcher>();
    }

    public CommandRegistryBuilder RegisterCommands<TCommandAttribute>(params Assembly[] assemblies) where TCommandAttribute : Attribute, ICommandAttribute
    {
        foreach (Assembly assembly in assemblies)
        {
            foreach (var (type, attr) in CollectCommandTypes<TCommandAttribute>(assembly))
            {
                collection.Register(new string[] { attr.Name }, type);
                collection.Register(attr.Aliases ?? new string[0], type);
                IoCBuilderAdapter.RegisterTransient(type);
            }
        }

        return this;
    }

    public CommandRegistryBuilder Register<TCommandAttribute>(Type type) where TCommandAttribute : Attribute, ICommandAttribute
    {
        var attribute = type.GetCustomAttribute<TCommandAttribute>();

        if (attribute == null)
            throw new Exceptions.ECFException($"Type {type.FullName} doesn't have attribute named {typeof(TCommandAttribute).FullName}");

        collection.Register(new string[] { attribute.Name }, type);
        if (attribute.Aliases != null && attribute.Aliases.Length > 0)
            collection.Register(attribute.Aliases, type);

        IoCBuilderAdapter.RegisterTransient(type);

        return this;
    }

    public CommandRegistryBuilder Register(Type type, string commandBinding)
    {
        collection.Register(new[] { commandBinding }, type);
        IoCBuilderAdapter.RegisterTransient(type);

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
