using ECF.Tests.HighLevelTextTests.HighLevelTextTests;
using ECF.Tests.Mocks.Commands;

namespace ECF.Tests.HighLevelTextTests;

[Collection("ConsoleTests")]
public class CommandHelp
{
    private readonly ConsoleSteamsFixture consoleStreams;

    public CommandHelp(ConsoleSteamsFixture consoleStreams)
    {
        this.consoleStreams = consoleStreams;
    }

    [Fact]
    public async Task help_flag_is_working()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("command-with-one-argument -h");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = new CommandWithOneArgument().GetHelp() + "\n";

        expectedOutput = expectedOutput.Replace("\r\n", "\n");

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task help_flag_will_ignore_other_arguments()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("command-with-one-argument argument --help");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = new CommandWithOneArgument().GetHelp() + "\n";

        expectedOutput = expectedOutput.Replace("\r\n", "\n");

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task overriden_help_will_replace_default_behaviour()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("command-with-one-argument-with-overriden-help argument -h");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        Assert.Equal("Argument:argument|Help:True\n", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task overrides_only_apply_to_some_of_default_names()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("command-with-one-argument-with-overriden-help --help"); // --help is not overriden
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = new CommandWithOneArgumentAndOverridernHelp().GetHelp() + "\n";

        expectedOutput = expectedOutput.Replace("\r\n", "\n");

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }
}
