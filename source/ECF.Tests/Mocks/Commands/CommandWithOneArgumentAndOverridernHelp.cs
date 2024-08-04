namespace ECF.Tests.Mocks.Commands;

[Command("command-with-one-argument-with-overriden-help")]
public class CommandWithOneArgumentAndOverridernHelp : CommandBase
{
    [Required, Argument(0)]
    public string? Argument { get; set; }

    [Flag("-h")]
    public bool Help { get; set; }

    public override void Execute()
    {
        Console.WriteLine($"Argument:{Argument}|Help:{Help}");
    }
}
