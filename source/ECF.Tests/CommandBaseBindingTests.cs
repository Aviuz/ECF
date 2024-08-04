using ECF.Tests.Mocks.Commands;

namespace ECF.Tests;

public class CommandBaseBindingTests
{
    [Fact]
    public void all_new_bindings_works()
    {
        BindingCommand command = new();

        var args = "first second third -f1 -f2 f3 -p1 fourth -p2 fifth p3 sixth".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.Equal("first", command.Argument1);
        Assert.Equal("second", command.Argument2);
        Assert.Equal("third", command.Argument3);

        Assert.True(command.Flag1);
        Assert.True(command.Flag2);
        Assert.True(command.Flag3);

        Assert.Equal("fourth", command.Parameter1);
        Assert.Equal("fifth", command.Parameter2);
        Assert.Equal("sixth", command.Parameter3);
    }

    [Fact]
    public void all_old_bindings_works()
    {
        LegacyBindingCommand command = new();

        var args = "first second third -cf1 -cf2 -cf3 -cp1 fourth -cp2 fifth -cp3 sixth".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.Equal("first", command.CmdArgument1);
        Assert.Equal("second", command.CmdArgument2);
        Assert.Equal("third", command.CmdArgument3);

        Assert.True(command.CmdFlag1);
        Assert.True(command.CmdFlag2);
        Assert.True(command.CmdFlag3);

        Assert.Equal("fourth", command.CmdParameter1);
        Assert.Equal("fifth", command.CmdParameter2);
        Assert.Equal("sixth", command.CmdParameter3);
    }

    [Fact]
    public void priorize_new_bindings_over_old_ones()
    {
        // note:    command.Argument1 can accept '-' prefix
        //          command.CmdArgument2 can accept '-' prefix
        //          all other arguments ignore tokens with '-' prefixes

        BindingCommand command = new();

        var args = "first -second third".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.Equal("first", command.Argument1);
        Assert.Null(command.Argument2);
        Assert.Equal("third", command.Argument3);

        Assert.Null(command.CmdArgument1);
        Assert.Equal("-second", command.CmdArgument2);
        Assert.Null(command.CmdArgument3);
    }

    [Fact]
    public void arguments_in_right_order()
    {
        BindingCommand command = new();

        var args = "first second third fourth fifth -f1 -f2 -f3".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.Equal("first", command.Argument1);
        Assert.Equal("second", command.Argument2);
        Assert.Equal("third", command.Argument3);
    }

    [Fact]
    public void case_sensitive_tests()
    {
        BindingCommand command = new();

        var args = "-f1 -F2 f3 -p1 test -P2 test p3 test -cf1 -cF2 -cf3 -cp1 test -CP2 test -cp3 test".Split();

        command.ApplyArguments(new CommandArguments(null, args));

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
    public void avoid_binding_by_accident_another_flag()
    {
        BindingCommand command = new();

        var args = "justtoignorefistargument -p1 -p2 test".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.Null(command.Parameter1);
        Assert.Equal("test", command.Parameter2);
    }

    [Fact]
    public void case_insensitive_tests()
    {
        BindingCommand_CaseInsensitve command = new();

        var args = "-f1 -F2 f3 -p1 test -P2 test p3 test".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.True(command.Flag1);
        Assert.True(command.Flag2);
        Assert.True(command.Flag3);

        Assert.Equal("test", command.Parameter1);
        Assert.Equal("test", command.Parameter2);
        Assert.Equal("test", command.Parameter3);
    }

    [Theory]
    [InlineData("", null, null, null, false, false, false, null, null, null, false, false, false, null, null, null)]
    [InlineData("first second third fourth fifth -f1 -f2 -f3", "first", "second", "third", true, true, false, null, null, null, false, false, false, null, null, null)]
    [InlineData("-f1 first -f2 -f3", "first", null, null, true, true, false, null, null, null, false, false, false, null, null, null)]
    [InlineData("-p1 -p2 test", "-p1", null, null, false, false, false, null, "test", null, false, false, false, null, null, null)]
    [InlineData("-p1 hey -p2 test", null, null, null, false, false, false, "hey", "test", null, false, false, false, null, null, null)]
    [InlineData("first -p1 hey -p2 test", "first", null, null, false, false, false, "hey", "test", null, false, false, false, null, null, null)]
    public void bonus_tests(string input, string? argument1, string? argument2, string? argument3, bool flag1, bool flag2, bool flag3, string? parameter1, string? parameter2, string? parameter3, bool legacy_flag1, bool legacy_flag2, bool legacy_flag3, string? legacy_parameter1, string? legacy_parameter2, string? legacy_parameter3)
    {
        BindingCommand command = new();

        var args =
            !string.IsNullOrEmpty(input)
            ? input.Split()
            : Array.Empty<string>();

        command.ApplyArguments(new CommandArguments(null, args));

        // new bindings
        Assert.Equal(argument1, command.Argument1);
        Assert.Equal(argument2, command.Argument2);
        Assert.Equal(argument3, command.Argument3);

        Assert.Equal(flag1, command.Flag1);
        Assert.Equal(flag2, command.Flag2);
        Assert.Equal(flag3, command.Flag3);

        Assert.Equal(parameter1, command.Parameter1);
        Assert.Equal(parameter2, command.Parameter2);
        Assert.Equal(parameter3, command.Parameter3);

        // legacy bindings
        Assert.Equal(legacy_flag1, command.CmdFlag1);
        Assert.Equal(legacy_flag2, command.CmdFlag2);
        Assert.Equal(legacy_flag3, command.CmdFlag3);

        Assert.Equal(legacy_parameter1, command.CmdParameter1);
        Assert.Equal(legacy_parameter2, command.CmdParameter2);
        Assert.Equal(legacy_parameter3, command.CmdParameter3);
    }
}
