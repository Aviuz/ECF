namespace ECF.Tests.Mocks.Commands;

[Command("command-with-aliases", "aliaso", "commandoo")] // with aliases
public class CommandWithAliases : CommandBase
{
    public static string ExpectedOutput = "CommandWithAliases executed";
    public override void Execute() => Console.WriteLine(ExpectedOutput);
}
