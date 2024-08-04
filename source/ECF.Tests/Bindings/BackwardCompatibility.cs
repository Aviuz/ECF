using ECF.Tests.Mocks.Commands;

namespace ECF.Tests.Bindings;

public class BackwardCompatibility
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
}
