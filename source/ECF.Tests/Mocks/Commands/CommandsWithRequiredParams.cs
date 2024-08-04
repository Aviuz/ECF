namespace ECF.Tests.Mocks.Commands;

[Command("required-test")]
public class CommandsWithRequiredParams : CommandBase
{
    [Required, Argument(0, Name = "required_argument")]
    public string? RequiredArgument { get; set; }

    [Argument(1, Name = "non_required_argument")]
    public string? NonRequiredArgument { get; set; }

    [Required, Parameter("--required-param")]
    public string? RequiredParameter { get; set; }

    [Parameter("--non-required-param")]
    public string? NonRequiredParameter { get; set; }

    public override void Execute()
    {
        Console.WriteLine($"RequiredArgument:{RequiredArgument}|NonRequiredArgument:{NonRequiredArgument}|RequiredParameter:{RequiredParameter}|NonRequiredParameter:{NonRequiredParameter}");
    }
}
