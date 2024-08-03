namespace ECF.Tests.Mocks;

#pragma warning disable CS0618 // Type or member is obsolete
[Command("old-flags-binding")]
internal class CommandWithFlagDefinedInOldWay : CommandBase
{
    [Flag(ShortName = "s")]
    public bool Short { get; set; }

    [Flag(LongName = "long")]
    public bool Long { get; set; }

    [Flag(LongName = "mixed", ShortName = "m")]
    public bool Mixed { get; set; }

    public override void Execute()
    {
        Console.WriteLine($"Short:{Short}|Long:{Long}|Mixed:{Mixed}");
    }
}
#pragma warning restore CS0618 // Type or member is obsolete

[Command("new-flags-binding")]
public class CommandWithFlagDefinedInNewWay : CommandBase
{
    [Flag("-m", "--mixed", "~mixed")]
    public bool Parameter { get; set; }

    public override void Execute()
    {
        Console.WriteLine(Parameter);
    }
}