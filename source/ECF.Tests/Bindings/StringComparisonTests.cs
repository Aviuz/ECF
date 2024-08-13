using ECF.Tests.Mocks.Commands;

namespace ECF.Tests.Bindings;

public class StringComparisonTests
{
    [Fact]
    public void case_sensitive_tests()
    {
        BindingCommand command = new();

        var args = "-f1 -F2 f3 -p1 test -P2 test p3 test -cf1 -cF2 -cf3 -cp1 test -CP2 test -cp3 test".Tokenize();

        command.ApplyArguments(CommandArguments.FromCode(args));

        Assert.True(command.Flag1);
        Assert.False(command.Flag2);
        Assert.True(command.Flag3);

        Assert.Equal("test", command.Parameter1);
        Assert.Null(command.Parameter2);
        Assert.Equal("test", command.Parameter3);

        Assert.True(command.CmdFlag1);
        Assert.False(command.CmdFlag2);
        Assert.True(command.CmdFlag3);

        Assert.Equal("test", command.CmdParameter1);
        Assert.Null(command.CmdParameter2);
        Assert.Equal("test", command.CmdParameter3);
    }

    [Fact]
    public void case_insensitive_tests()
    {
        BindingCommand_CaseInsensitve command = new();

        var args = "-f1 -F2 f3 -p1 test -P2 test p3 test".Tokenize();

        command.ApplyArguments(CommandArguments.FromCode(args));

        Assert.True(command.Flag1);
        Assert.True(command.Flag2);
        Assert.True(command.Flag3);

        Assert.Equal("test", command.Parameter1);
        Assert.Equal("test", command.Parameter2);
        Assert.Equal("test", command.Parameter3);
    }
}
