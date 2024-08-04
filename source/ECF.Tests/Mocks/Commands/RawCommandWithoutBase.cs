namespace ECF.Tests.Mocks.Commands;

[Command("raw-dog")]
public class RawCommandWithoutBase : ICommand
{
    public static string ExpectedOutput = "Custom command executed";

    public void ApplyArguments(CommandArguments args) { }

    public Task ExecuteAsync(CancellationToken _)
    {
        Console.WriteLine(ExpectedOutput);
        return Task.CompletedTask;
    }
}