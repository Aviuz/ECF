using ECF.InverseOfControl;

namespace ECF.Utilities;

public class CommandDispatcher
{
    private readonly IIoCProviderAdapter iocProvider;

    public CommandDispatcher(IIoCProviderAdapter iocProvider)
    {
        this.iocProvider = iocProvider;
    }

    public void ExecuteCommand<T>(params string[] commandArgs) where T : ICommand => ExecuteCommandAsync<T>(commandArgs).Wait();

    public async Task ExecuteCommandAsync<T>(string[] commandArgs, CancellationToken cancellationToken = default) where T : ICommand
    {
        using (var nestedScope = iocProvider.BeginNestedScope())
        {
            var command = nestedScope.Resolve<T>();
            await command.ExecuteAsync(CommandArguments.FromCode(commandArgs), cancellationToken);
        }
    }
}
