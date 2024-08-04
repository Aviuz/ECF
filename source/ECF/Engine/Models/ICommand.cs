namespace ECF;

public interface ICommand
{
    Task ExecuteAsync(CommandArguments args, CancellationToken cancellationToken);
}
