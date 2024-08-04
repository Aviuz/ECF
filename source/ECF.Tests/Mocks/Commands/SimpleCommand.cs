namespace ECF.Tests.Mocks.Commands;

[Command("simple")]
public class SimpleCommand : CommandBase
{
    public static string ExpectedOutput = "Simple command executed";
    public override void Execute() => Console.WriteLine(ExpectedOutput);
}
