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

        string expectedOutput = new CommandWithOneArgument().GetHelp().RemoveNoise() + "\n";

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

        string expectedOutput = new CommandWithOneArgument().GetHelp().RemoveNoise() + "\n";

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

        string expectedOutput = new CommandWithOneArgumentAndOverridernHelp().GetHelp().RemoveNoise() + "\n";

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task help_command_without_arguments_displays_intro_text_and_all_commands()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("help");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .Configure((ctx, _, _) =>
            {
                ctx.HelpIntro = "6572191373281";
            })
            .RunAsync([]);

        Assert.StartsWith("6572191373281", consoleStreams.GetConsoleOutput());
        Assert.Contains("command-with-one-argument-with-overriden-help", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task help_command_also_displays_help_for_specified_command()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("help command-with-one-argument-with-overriden-help");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .Configure((ctx, _, _) =>
            {
                ctx.HelpIntro = "548487521848";
            })
            .RunAsync([]);

        string expectedOutput = new CommandWithOneArgumentAndOverridernHelp().GetHelp().RemoveNoise() + "\n";

        Assert.DoesNotContain("548487521848", consoleStreams.GetConsoleOutput());
        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task executing_help_with_unknown_command_should_give_specified_output_format()
    {
        string commandNotFoundFormat = "There is no {0} command. Type help to list commands.";
        string commandName = "this-command-does-not-exists-and-to-be-sure-lets-put-some-numbers-287361831293172391";

        consoleStreams.Reset();

        consoleStreams.UserInput($"help {commandName}");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Format(commandNotFoundFormat, commandName).RemoveNoise() + "\n";

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task executing_unknown_command_should_give_specified_output_format()
    {
        string commandNotFoundFormat = "Command not found: {0}. Type 'help' to list commands.";
        string commandName = "this-command-does-not-exists-and-to-be-sure-lets-put-some-numbers-287361831293172391";

        consoleStreams.Reset();

        consoleStreams.UserInput($"{commandName}");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Format(commandNotFoundFormat, commandName).RemoveNoise() + "\n";

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }
}
