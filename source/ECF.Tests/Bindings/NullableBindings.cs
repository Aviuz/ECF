namespace ECF.Tests.Bindings;

public class NullableBindings
{
    [Fact]
    public void assigning_nullable_and_nonnulable_works_same()
    {
        NullableCommand command = new();

        var args = "134 \"7820\"".Tokenize();

        command.ApplyArguments(CommandArguments.FromCode(args));

        Assert.Equal(134, command.NonNullableProperty);
        Assert.Equal(7820, command.NullableProperty);
    }

    [Fact]
    public void defaults_working_as_intended()
    {
        NullableCommand command = new();

        command.ApplyArguments(CommandArguments.FromCode([]));

        Assert.Equal(0, command.NonNullableProperty);
        Assert.Null(command.NullableProperty);
    }
}

public class NullableCommand : CommandBase
{
    [Argument(0)]
    public int NonNullableProperty { get; set; }

    [Argument(1)]
    public int? NullableProperty { get; set; }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
