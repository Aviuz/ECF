namespace ECF.Tests.Mocks.Commands;

[Command("command-with-one-parameter")]
public class CommandWithOneParameter : CommandBase
{
    [Required, Parameter("-p --parameter")]
    public string? Parameter { get; set; }

    public override void Execute()
    {
        Console.WriteLine($"Parameter:{Parameter}");
    }
}