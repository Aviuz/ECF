using Autofac;
using ECF.Autofac.Adapters;
using ECF.Engine;
using ECF.Tests.Mocks.Commands;
using System.Reflection;

namespace ECF.Tests;

public class EngineTests
{
    [Fact]
    public void ensure_all_commands_are_returned_from_ICommandCollection()
    {
        ContainerBuilder containerBuilder = new();
        AutofacContainerBuilderAdapter autofacContainerBuilderAdapter = new(containerBuilder);
        CommandRegistryBuilder registryBuilder = new(autofacContainerBuilderAdapter);


        registryBuilder.RegisterCommands<CommandAttribute>(Assembly.GetExecutingAssembly());

        var container = containerBuilder.Build();
        var collection = container.Resolve<ICommandCollection>();

        Assert.Equal(typeof(CommandWithAliases), collection.GetCommand("command-with-aliases"));
        Assert.Equal(typeof(CommandWithAliases), collection.GetCommand("aliaso"));
        Assert.Equal(typeof(CommandWithAliases), collection.GetCommand("commandoo"));
        Assert.Equal(typeof(SimpleCommand), collection.GetCommand("simple"));
        Assert.Equal(typeof(RawCommandWithoutBase), collection.GetCommand("raw-dog"));

        Assert.DoesNotContain(typeof(CommandWithoutAttribute), collection.GetAllCommands());
    }
}