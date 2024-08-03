using ECF.Exceptions;
using ECF.InverseOfControl;
using ECF.Utilities;

namespace ECF.Engine;

/// <summary>
/// This class is responsible for processing input collected from user. It will parse input and execute command based on input.
/// </summary>
public interface ICommandProcessor
{
    Task ProcessAsync(string? commandLine, CancellationToken ct);
    Task ProcessAsync(string[] args, CancellationToken ct);
}

/// <inheritdoc cref="ICommandProcessor" />
public class CommandProcessor : ICommandProcessor
{
    private readonly IIoCProviderAdapter iocProvider;

    public CommandProcessor(IIoCProviderAdapter iocProvider)
    {
        this.iocProvider = iocProvider;
    }

    public Task ProcessAsync(string? commandLine, CancellationToken ct) => ProcessAsync(CommandLineUtilities.CommandLineToArgs(commandLine), ct);

    public async Task ProcessAsync(string[] args, CancellationToken ct)
    {
        using (var scope = iocProvider.BeginNestedScope())
        {
            var resolver = scope.Resolve<ICommandResolver>();
            var context = scope.Resolve<InterfaceContext>();

            ICommand? command;
            CommandArguments arguments;

            if (args.Length > 0)
            {
                command = resolver.Resolve(args[0]);
                arguments = new CommandArguments(
                    commandName: args[0],
                    arguments: args.Skip(1).ToArray()
                );
            }
            else
            {
                command = null;
                arguments = new CommandArguments(
                    commandName: null,
                    arguments: args ?? new string[0]
                );
            }

            if (command == null && context.DefaultCommand != null)
            {
                command = resolver.Resolve(context.DefaultCommand);
                arguments.IsFallbackRequested = true;
            }

            if (command == null)
            {
                throw new CommandNotFoundException(args ?? new string[0]);
            }

            command.ApplyArguments(arguments);
            await command.ExecuteAsync(ct);
        }
    }
}