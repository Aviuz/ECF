using ECF.InverseOfControl;

namespace ECF.Utilities;

public class CommandDispatcher
{
    private readonly IIoCScopeAdapter scope;

    public CommandDispatcher(IIoCScopeAdapter scope)
    {
        this.scope = scope;
    }

    public void ExecuteCommand<T>(params string[] commandArgs) where T : ICommand
    {
        using (var nestedScope = scope.BeginNestedScope())
        {
            var command = nestedScope.Resolve<T>();
            command.ApplyArguments(new CommandArguments() { Arguments = commandArgs });
            command.Execute();
        }
    }
}
