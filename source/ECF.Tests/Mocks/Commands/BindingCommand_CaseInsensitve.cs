namespace ECF.Tests.Mocks.Commands;

#pragma warning disable CS0618 // Type or member is obsolete, Reason: we're checking backward compatibility
[Command("binding-command", "bc")]
public class BindingCommand_CaseInsensitve : CommandBase
{
    public static string ExpectedOutput = "Binding command executed";

    [Argument(2)] public string? Argument3 { get; set; }
    [Argument(0, IgnorePrefixes = null)] public string? Argument1 { get; set; }
    [Argument(1)] public string? Argument2 { get; set; }

    [Flag("-f1 --flag1", ComparisonMode = StringComparison.InvariantCultureIgnoreCase)] public bool Flag1 { get; set; }
    [Flag("-f2", "--flag2", ComparisonMode = StringComparison.InvariantCultureIgnoreCase)] public bool Flag2 { get; set; }
    [Flag("f3", ComparisonMode = StringComparison.InvariantCultureIgnoreCase)] public bool Flag3 { get; set; }

    [Parameter("-p1 --param1", ComparisonMode = StringComparison.InvariantCultureIgnoreCase)] public string? Parameter1 { get; set; }
    [Parameter("-p2", "--param2", ComparisonMode = StringComparison.InvariantCultureIgnoreCase)] public string? Parameter2 { get; set; }
    [Parameter("p3", ComparisonMode = StringComparison.InvariantCultureIgnoreCase)] public string? Parameter3 { get; set; }

    public override void Execute() => Console.WriteLine(ExpectedOutput);
}
#pragma warning restore CS0618 // Type or member is obsolete