using ECF;
using ECF.Commands;
using ECF.Engine;
using System.Reflection;

namespace Example
{
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

    public class DirectionCommandsScope : ICommandScope
    {
        public ICommandProcesor Processor { get; }

        public DirectionCommandsScope(InterfaceContext interfaceContext)
        {
            // when using alternative scope, we need to build command registry manually
            var registryBuilder = new CommandRegistryBuilder(interfaceContext);
            // we can register all commands with specified attribute in specified assembly
            registryBuilder.RegisterCommands<DirectionCommandAttribute>(Assembly.GetExecutingAssembly());

            // this line will register basic commands as HelpCommand, LoadCommand etc.
            registryBuilder.RegisterCommands<CommandAttribute>(typeof(HelpCommand).Assembly);

            // alternatively you can always register commands seperatly one by one
            registryBuilder.Register<CommandAttribute>(typeof(ExitCommand));

            // at the end we need to construct CommandProcesor which will process command requests
            Processor = new CommandProcessor(registryBuilder.ContainerBuilder.Build());
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
            interfaceContext.CommandScope = new DirectionCommandsScope(interfaceContext);
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
}
