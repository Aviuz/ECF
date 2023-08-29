using ECF.InverseOfControl;
using ECF.Utilities;

namespace ECF.Engine;

public interface ICommandProcesor
{
    void Process(string? commandLine);
    void Process(string[] args);
}

public class CommandProcessor : ICommandProcesor
{
    private readonly IIoCProviderAdapter iocProvider;

    public CommandProcessor(IIoCProviderAdapter iocProvider)
    {
        this.iocProvider = iocProvider;
    }

    public void Process(string? commandLine) => Process(CommandLineUtilities.CommandLineToArgs(commandLine));

    public void Process(string[] args)
    {
        using (var scope = iocProvider.GetScope())
        {
            var resolver = scope.Resolve<ICommandResolver>();

            ICommand command = resolver.CreateCommand(ParseArguments(args));

            command.Execute();
        }
    }

    private CommandArguments ParseArguments(string[] args)
    {
        return new CommandArguments()
        {
            CommandName = args[0],
            Arguments = args.Skip(1).ToArray(),
        };
    }
}