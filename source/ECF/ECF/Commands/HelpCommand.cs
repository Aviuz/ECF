using Autofac;
using ECF.Engine;

namespace ECF.Commands
{
    [Command("help")]
    public class HelpCommand : ICommand
    {
        private string? commandName;

        private readonly ICommandResolver commandResolver;
        private readonly InterfaceContext interfaceContext;
        private readonly ILifetimeScope lifetimeScope;

        public HelpCommand(ILifetimeScope lifetimeScope, ICommandResolver commandResolver, InterfaceContext interfaceContext)
        {
            this.lifetimeScope = lifetimeScope;
            this.commandResolver = commandResolver;
            this.interfaceContext = interfaceContext;
        }

        public void ApplyArguments(CommandArguments args)
        {
            if (args.Arguments.Length > 0)
            {
                commandName = args.Arguments[0];
            }
        }

        public void Execute()
        {
            if (commandName == null)
            {
                PrintAvailableCommands();
            }
            else
            {
                var command = commandResolver.Resolve(commandName);

                if (command == null)
                {
                    Console.WriteLine($"There is no {commandName} command. Type help to list commands.");
                    return;
                }

                var helpCommand = command as IHaveHelp;

                if (helpCommand == null)
                {
                    Console.WriteLine($"Command {commandName} is not implementing {nameof(IHaveHelp)} interface");
                    return;
                }

                Console.WriteLine(helpCommand.GetHelp());
            }
        }

        private void PrintAvailableCommands()
        {
            if (!string.IsNullOrEmpty(interfaceContext.HelpIntro))
            {
                Console.WriteLine(interfaceContext.HelpIntro);
                Console.WriteLine();
            }

            Console.WriteLine("Available commands:");
            foreach (var command in GetCommands())
            {
                if (command.Aliases?.Length > 0)
                    Console.WriteLine($"\t{command.Name} ({string.Join(", ", command.Aliases)})");
                else
                    Console.WriteLine($"\t{command.Name}");
            }

            Console.WriteLine();
            Console.WriteLine("Type 'help <command>' for more information about specific command");
        }

        private IEnumerable<ICommandAttribute> GetCommands()
        {
            var commandAttributes = commandResolver
                    .GetAllCommands()
                    .OrderBy(a => a.Name);

            foreach (var attr in commandAttributes)
            {
                yield return attr;
            }
        }
    }
}
