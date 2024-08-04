using ECF.Engine;
using System.Reflection;

namespace ECF.BaseKitCommands;

[Command("help", "--help", "-h")]
public class HelpCommand : ICommand, IHaveHelp
{
    private string? displayHelpForCommand;
    private string? notFoundCommand;

    private readonly ICommandResolver commandResolver;
    private readonly ICommandCollection commandsCollection;
    private readonly InterfaceContext interfaceContext;

    public HelpCommand(ICommandResolver commandResolver, ICommandCollection commandsCollection, InterfaceContext interfaceContext)
    {
        this.commandResolver = commandResolver;
        this.commandsCollection = commandsCollection;
        this.interfaceContext = interfaceContext;
    }

    public Task ExecuteAsync(CommandArguments args, CancellationToken _)
    {
        if (args.IsFallbackRequested && string.IsNullOrWhiteSpace(args.CommandName) == false) // command with binding args.CommandName does not exits 
        {
            Console.WriteLine($"Command not found: {notFoundCommand}. Type 'help' to list commands.");
        }
        else if (args.Arguments.Length > 0 && string.IsNullOrWhiteSpace(args.Arguments[0]) == false) // for example: help command-to-display-help
        {
            PrintHelpForSpecifiedCommand(args.Arguments[0]);
        }
        else
        {
            PrintAvailableCommands();
        }

        return Task.CompletedTask;
    }

    private void PrintHelpForSpecifiedCommand(string commandName)
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

    private void PrintAvailableCommands()
    {
        if (!string.IsNullOrEmpty(interfaceContext.HelpIntro))
        {
            Console.WriteLine(interfaceContext.HelpIntro);
            Console.WriteLine();
        }

        ComandHelpResultList commandsList = new();
        commandsList.OtherCommands = GetCommandsRegisteredInCollection().ToList();
        UpdateDefaultCommand(commandsList);

        if (commandsList.DefaultCommands?.Any() == true)
        {
            Console.WriteLine("Default command:");
            foreach (var command in commandsList.DefaultCommands)
                DisplaySingleCommandResult(command);

            Console.WriteLine();

            Console.WriteLine("Other commands:");
            foreach (var command in commandsList.OtherCommands)
                DisplaySingleCommandResult(command);
        }
        else
        {
            Console.WriteLine("Available commands:");
            foreach (var command in commandsList.OtherCommands)
                DisplaySingleCommandResult(command);
        }

        Console.WriteLine();
        Console.WriteLine("Type 'help <command>' for more information about specific command");
    }

    private void DisplaySingleCommandResult(CommandHelpResult command)
    {
        if (command.Name != null && command.Aliases?.Length > 0)
            Console.WriteLine($"\t{command.Name} ({string.Join(", ", command.Aliases)})");
        else if (command.Name != null)
            Console.WriteLine($"\t{command.Name}");
        else if (command.Aliases?.Length > 0)
            Console.WriteLine($"\t{string.Join(", ", command.Aliases)}");
        else
            Console.WriteLine($"\t{command.Type.Name}");
    }

    private IEnumerable<CommandHelpResult> GetCommandsRegisteredInCollection()
    {
        List<CommandHelpResult> results = new();

        foreach (Type commandType in commandsCollection.GetAllCommands())
        {
            // Note: here we potentially getting multiple attributes for the same type
            // which can lead to duplicates in the output - I accept this since why would you even put multiple tags :)
            var commandAttributes = commandType
                .GetCustomAttributes()
                .Where(attr => attr is ICommandAttribute)
                .Cast<ICommandAttribute>();

            foreach (var attr in commandAttributes)
            {
                CommandHelpResult result = new(commandType);

                // This check is to ensure developer didn't mess up with the registration
                if (commandsCollection.GetCommand(attr.Name) == commandType)
                    result.Name = attr.Name;

                result.Aliases = attr.Aliases?
                        .Where(alias => commandsCollection.GetCommand(alias) == commandType)
                        .ToArray()
                    ?? new string[0];

                if (result.Name != null || result.Aliases.Length > 0)
                    results.Add(result);
            }
        }

        return results.OrderBy(r => r.Name);
    }

    private void UpdateDefaultCommand(ComandHelpResultList list)
    {
        if (interfaceContext.DefaultCommand == null || interfaceContext.DefaultCommand == typeof(HelpCommand))
            return;

        var defaultCommandsInList = list.OtherCommands!.Where(c => c.Type == interfaceContext.DefaultCommand);

        if (defaultCommandsInList?.Any() == true)
        {
            list.DefaultCommands = defaultCommandsInList.ToList();
            list.OtherCommands = list.OtherCommands!.Except(defaultCommandsInList).ToList();
        }
        else
        {
            list.DefaultCommands = new List<CommandHelpResult> { new(interfaceContext.DefaultCommand) };
        }
    }

    public string GetHelp()
    {
        return @"To use command type 'help <command>' to get more information about specific command.";
    }

    private class ComandHelpResultList
    {
        // Multiple becase there is a possibility of multiple default command attributes, but only one type
        public List<CommandHelpResult>? DefaultCommands { get; set; }
        public List<CommandHelpResult>? OtherCommands { get; set; }
    }

    private class CommandHelpResult
    {
        public string? Name { get; set; }
        public string[] Aliases { get; set; } = Array.Empty<string>();
        public Type Type { get; init; }

        public CommandHelpResult(Type type)
        {
            Type = type;
        }
    }
}