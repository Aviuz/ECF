using ECF.Tests.HighLevelTextTests.HighLevelTextTests;

namespace ECF.Tests.HighLevelTextTests;

[Collection("ConsoleTests")]
public class RequiredBindingChange
{
    private readonly ConsoleSteamsFixture consoleStreams;

    public RequiredBindingChange(ConsoleSteamsFixture consoleStreams)
    {
        this.consoleStreams = consoleStreams;
    }

    [Fact]
    public async Task when_passed_without_required_arguments_display_message_instead_of_execute()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("required-test");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", [
            "Argument <required_argument> is required",
            "Parameter --required-param is required",
            "",
            "CommandSyntax: <required_argument> [<non_required_argument>] --required-param <value> [--non-required-param <value>]"
        ]) + '\n';

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task when_missing_only_one_display_message_only_for_missing()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("required-test firstArgument");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", [
            "Parameter --required-param is required",
            "",
            "CommandSyntax: <required_argument> [<non_required_argument>] --required-param <value> [--non-required-param <value>]"
        ]) + '\n';

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task when_all_required_are_supplied_it_executes()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("required-test firstArgument --required-param test");
        consoleStreams.UserInput("required-test firstArgument --required-param test --non-required-param nonreq");
        consoleStreams.UserInput("required-test firstArgument --required-param test second --non-required-param nonreq");
        consoleStreams.UserInput("required-test --required-param test firstArgument second --non-required-param nonreq");
        consoleStreams.UserInput("required-test --non-required-param nonreq --required-param test firstArgument second");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", [
            "RequiredArgument:firstArgument|NonRequiredArgument:|RequiredParameter:test|NonRequiredParameter:",
            "RequiredArgument:firstArgument|NonRequiredArgument:|RequiredParameter:test|NonRequiredParameter:nonreq",
            "RequiredArgument:firstArgument|NonRequiredArgument:second|RequiredParameter:test|NonRequiredParameter:nonreq",
            "RequiredArgument:firstArgument|NonRequiredArgument:second|RequiredParameter:test|NonRequiredParameter:nonreq",
            "RequiredArgument:firstArgument|NonRequiredArgument:second|RequiredParameter:test|NonRequiredParameter:nonreq",
        ]) + '\n';

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }
}
