using ECF.Tests.Mocks.Commands;

namespace ECF.Tests.Bindings;

public class RequiredPropertiesBinding
{
    [Fact]
    public void empty_should_return_false_in_Validate()
    {
        CommandsWithRequiredParams command = new();

        var args = new string[0];

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.False(command.Validate(out var errors));

        Assert.NotNull(errors);
        Assert.Equal(2, errors.Count);
        Assert.Equal("Argument <required_argument> is required", errors[0]);
        Assert.Equal("Parameter --required-param is required", errors[1]);
    }

    [Fact]
    public void only_one_required_supplied_should_return_false_in_Validate()
    {
        CommandsWithRequiredParams command = new();

        var args = "first".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.False(command.Validate(out var errors));

        Assert.NotNull(errors);
        Assert.Single(errors);
        Assert.Equal("Parameter --required-param is required", errors[0]);
    }


    [Fact]
    public void minimum_supplied_should_return_true_in_Validate()
    {
        CommandsWithRequiredParams command = new();

        var args = "first --required-param test".Split();

        command.ApplyArguments(new CommandArguments(null, args));

        Assert.True(command.Validate(out var errors));

        Assert.Empty(errors);
    }

    [Fact]
    public void full_supplied_should_return_true_in_Validate()
    {
        CommandsWithRequiredParams command = new();

        var args = "first second --required-param test --non-required-param test".Split();

        command.ApplyArguments(new CommandArguments(null, args));
        Assert.True(command.Validate(out var errors));

        Assert.Empty(errors);
    }
}
