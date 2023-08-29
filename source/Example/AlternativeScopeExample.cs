using ECF;
using ECF.Commands;
using ECF.Engine;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Example;

// We can use diffrent attribute to seperate commands inside same assembly
public class DirectionCommandAttribute : Attribute, ICommandAttribute
{
    public string[]? Aliases { get; set; }

    public string Name { get; set; }

    public DirectionCommandAttribute(string name)
    {
        Name = name;
    }
}

// This command will switch to new scope. It's need to be visible inside previous scope.
[Command("directions")]
public class SwitchToDirectionContextCommand : CommandBase
{
    private readonly InterfaceContext interfaceContext;

    public SwitchToDirectionContextCommand(InterfaceContext interfaceContext)
    {
        this.interfaceContext = interfaceContext;
    }

    public override void Execute()
    {
        interfaceContext.Prefix = "$"; // we can also change prefix
        interfaceContext.CommandProcessor = CreateDirectionCommandsProcessor(interfaceContext);
    }

    public ICommandProcessor CreateDirectionCommandsProcessor(InterfaceContext interfaceContext)
    {
        // CommandProcessors hold IoC containers to maintain seperate collection of services
        var services = new ServiceCollection();
        services.AddSingleton(interfaceContext); // remember to always include interfaceContext inside IoC

        services
            .AddECFCommandRegistry() // when using alternative scope, we need to build command registry manually
            .RegisterCommands<DirectionCommandAttribute>(Assembly.GetExecutingAssembly()) // we can register all commands with specified attribute in specified assembly
            .RegisterCommands<CommandAttribute>(typeof(HelpCommand).Assembly) // this line will register basic commands as HelpCommand, LoadCommand etc.
            .Register<CommandAttribute>(typeof(ExitCommand)); // alternatively you can always register commands seperatly one by one

        // at the end we need to construct CommandProcesor which will process command requests
        return services.BuildAndCreateECFCommandProcessor();
    }
}

[DirectionCommand("left")]
public class LeftCommand : ICommand
{
    public void ApplyArguments(CommandArguments args) { }

    public void Execute()
    {
        Console.WriteLine("<<<<<");
    }
}

[DirectionCommand("right")]
public class RightCommand : ICommand
{
    public void ApplyArguments(CommandArguments args) { }

    public void Execute()
    {
        Console.WriteLine(">>>>>>");
    }
}
