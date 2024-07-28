namespace ECF;

public abstract class AsyncCommandBase : CommandBase
{
    public override sealed void Execute() => ExecuteAsync().Wait();

    public abstract Task ExecuteAsync();
}