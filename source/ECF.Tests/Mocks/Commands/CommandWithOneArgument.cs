namespace ECF.Tests.Mocks.Commands;

[Command("command-with-one-argument")]
public class CommandWithOneArgument : CommandBase
{
    [Required, Argument(0)]
    public string? Argument { get; set; }

    public override void Execute()
    {
        Console.WriteLine($"Argument:{Argument}");
    }
}
