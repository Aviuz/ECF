namespace ECF.Tests.Mocks.Commands;

[Command("raw-dog")]
public class RawCommandWithoutBase : ICommand
{
    public static string ExpectedOutput = "Custom command executed";

    public Task ExecuteAsync(CommandArguments args, CancellationToken cancellationToken)
    {
        Console.WriteLine(ExpectedOutput);
        return Task.CompletedTask;
    }
}