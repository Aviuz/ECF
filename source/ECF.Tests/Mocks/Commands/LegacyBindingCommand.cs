namespace ECF.Tests.Mocks.Commands;

#pragma warning disable CS0618 // Type or member is obsolete, Reason: we're checking backward compatibility
[Command("binding-command", "bc")]
[CmdArgument("cmdArg-3", 2)]
[CmdArgument("cmdArg-1", 0)]
[CmdArgument("cmdArg-2", 1, IgnorePrefixes = null)]
[CmdFlag("flag-1", ShortName = "cf1", LongName = "cmd-flag-1")]
[CmdFlag("flag-2", ShortName = "cf2", LongName = "cmd-flag-2")]
[CmdFlag("flag-3", ShortName = "cf3", LongName = "cmd-flag-3")]
[CmdParameter("param-1", ShortName = "cp1", LongName = "cmd-parameter-1")]
[CmdParameter("param-2", ShortName = "cp2", LongName = "cmd-parameter-2")]
[CmdParameter("param-3", ShortName = "cp3", LongName = "cmd-parameter-3")]
public class LegacyBindingCommand : CommandBase
{
    public static string ExpectedOutput = "Binding command executed";

    public string? CmdArgument1 => GetValue<string>("cmdArg-1");
    public string? CmdArgument2 => GetValue<string>("cmdArg-2");
    public string? CmdArgument3 => GetValue<string>("cmdArg-3");

    public bool CmdFlag1 => IsFlagActive("flag-1");
    public bool CmdFlag2 => IsFlagActive("flag-2");
    public bool CmdFlag3 => IsFlagActive("flag-3");

    public string? CmdParameter1 => GetValue<string>("param-1");
    public string? CmdParameter2 => GetValue<string>("param-2");
    public string? CmdParameter3 => GetValue<string>("param-3");

    public override void Execute() => Console.WriteLine(ExpectedOutput);
}
#pragma warning restore CS0618 // Type or member is obsolete