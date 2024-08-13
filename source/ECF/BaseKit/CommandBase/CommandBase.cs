namespace ECF;

public abstract class CommandBase : AsyncCommandBase
{
    public override sealed Task ExecuteAsync(CancellationToken ct)
    {
        ct.Register(() => {
            Environment.Exit(1);
        });

        Execute();
        return Task.CompletedTask;
    }

    public abstract void Execute();
}