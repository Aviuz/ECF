namespace ECF;

public interface ICommand
{
    void ApplyArguments(CommandArguments args);
    Task ExecuteAsync(CancellationToken cancellationToken);
}
