﻿using ECF.Engine;
using ECF.InverseOfControl;
using ECF.Utilities;
using System.Reflection;

namespace ECF;

public class CommandRegistryBuilder
{
    private readonly CommandCollection collection  = new CommandCollection();

    public IIoCBuilderAdapter IoCBuilderAdapter { get; }

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
                collection.Register(attr, type);
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

        collection.Register(attribute, type);
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
