using ECF.InverseOfControl;

namespace ECF.Utilities;

public class CommandDispatcher
{
    private readonly IIoCProviderAdapter iocProvider;

    public CommandDispatcher(IIoCProviderAdapter iocProvider)
    {
        this.iocProvider = iocProvider;
    }

    public void ExecuteCommand<T>(params string[] commandArgs) where T : ICommand
    {
        using (var nestedScope = iocProvider.BeginNestedScope())
        {
            var command = nestedScope.Resolve<T>();
            command.ApplyArguments(new CommandArguments() { Arguments = commandArgs });
            command.Execute();
        }
    }
}
