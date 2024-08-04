namespace ECF.Tests.Mocks.Commands;

[Command("nameless-arguments")]
public class NamelessArgumentsComamnd : CommandBase
{
    [Required, Argument(0)]
    public string? RequiredArgument { get; set; }

    [Argument(1)]
    public string? NonRequiredArgument { get; set; }

    public override void Execute()
    {
        Console.WriteLine($"RequiredArgument:{RequiredArgument}|NonRequiredArgument:{NonRequiredArgument}");
    }
}
