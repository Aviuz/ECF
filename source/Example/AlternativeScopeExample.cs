using ECF;
using ECF.BaseKitCommands;
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
        interfaceContext.DefaultCommand = typeof(HelpCommand); // we can also set default command (keep in mind that command need to be able to be resolved by IoC)
    }

    public ICommandProcessor CreateDirectionCommandsProcessor(InterfaceContext interfaceContext)
    {
        // CommandProcessors hold IoC containers to maintain seperate collection of services        

        return new ServiceCollection()
            // when using alternative scope, we need to build command registry manually
            .AddECFCommandRegistry(interfaceContext, builder => builder
                .RegisterCommands<DirectionCommandAttribute>(Assembly.GetExecutingAssembly()) // we can register all commands with specified attribute in specified assembly
                .RegisterCommands<CommandAttribute>(typeof(HelpCommand).Assembly) // this line will register basic commands as HelpCommand, LoadCommand etc.
                .Register<CommandAttribute>(typeof(ExitCommand)) // alternatively you can always register commands seperatly one by one
                .Register(typeof(ExitCommand), "exit")) // or even register without attribute (it can cause issues with help command)
            .BuildAndCreateECFCommandProcessor(); // at the end we need to construct CommandProcesor which will process command requests
    }
}

[DirectionCommand("left")]
public class LeftCommand : ICommand
{
    public Task ExecuteAsync(CommandArguments args, CancellationToken cancellationToken)
    {
        Console.WriteLine("<<<<<");
        return Task.CompletedTask;
    }
}

[DirectionCommand("right")]
public class RightCommand : ICommand
{
    public Task ExecuteAsync(CommandArguments args, CancellationToken cancellationToken)
    {
        Console.WriteLine(">>>>>>");
        return Task.CompletedTask;
    }
}
