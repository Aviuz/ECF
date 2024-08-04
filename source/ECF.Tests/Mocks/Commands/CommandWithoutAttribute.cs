namespace ECF.Tests.Mocks.Commands;

public class CommandWithoutAttribute : CommandBase
{
    public static string ExpectedOutput = "CommandWithoutAttribute executed";
    public override void Execute() => Console.WriteLine(ExpectedOutput);
}