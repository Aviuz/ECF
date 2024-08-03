namespace ECF;

public abstract class CommandBase : AsyncCommandBase
{
    public override sealed Task ExecuteAsync(CancellationToken _)
    {
        Execute();
        return Task.CompletedTask;
    }

    public abstract void Execute();
}