namespace ECF.Tests.Mocks;

#pragma warning disable CS0618 // Type or member is obsolete
[Command("old-parameters-binding")]
internal class CommandWithParameterDefinedInOldWay : CommandBase
{
    [Parameter(ShortName = "s")]
    public string? Short { get; set; }

    [Parameter(LongName = "long")]
    public string? Long { get; set; }

    [Parameter(LongName = "mixed", ShortName = "m")]
    public string? Mixed { get; set; }

    public override void Execute()
    {
        Console.WriteLine(string.Join("", [Short, Long, Mixed]));
    }
}
#pragma warning restore CS0618 // Type or member is obsolete

[Command("new-parameters-binding")]
public class CommandWithParameterDefinedInNewWay : CommandBase
{
    [Parameter("-m", "--mixed", "~mixed")]
    public string? Parameter { get; set; }

    public override void Execute()
    {
        Console.WriteLine(Parameter);
    }
}