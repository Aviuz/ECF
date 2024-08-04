using ECF.Exceptions;
using ECF.Tests.HighLevelTextTests.HighLevelTextTests;
using ECF.Tests.Mocks.Commands;
using System.Text;

namespace ECF.Tests.HighLevelTextTests;

[Collection("ConsoleTests")]
public class UsingECFHostBuilder
{
    private readonly ConsoleSteamsFixture consoleStreams;

    public UsingECFHostBuilder(ConsoleSteamsFixture consoleStreams)
    {
        this.consoleStreams = consoleStreams;
    }

    [Fact]
    public async Task intro_displayed_when_no_arguments_passed_to_program_and_command_matched()
    {
        consoleStreams.Reset();

        var hostTask = Task.Run(async () =>
            {
                await new ECFHostBuilder()
                    .UseDefaultCommands()
                    .Configure((ctx, _, _) =>
                    {
                        ctx.Intro = "73894759230";
                    })
                    .RunAsync([]);
            });

        consoleStreams.UserInput("simple");
        consoleStreams.UserInput("exit");

        await hostTask;

        Assert.Equal($"73894759230\n{SimpleCommand.ExpectedOutput}\n", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task cancelling_token_interrupts_input()
    {
        consoleStreams.Reset();

        var cancel = new CancellationTokenSource();

        var hostTask = Task.Run(async () =>
        {
            await new ECFHostBuilder()
                .UseDefaultCommands()
                .Configure((ctx, _, _) =>
                {
                    ctx.Intro = "73894759230";
                })
                .RunAsync([], cancel.Token);
        });

        cancel.Cancel();

        await hostTask;

        Assert.Equal(Encoding.UTF8.GetBytes($"73894759230\n"), Encoding.UTF8.GetBytes(consoleStreams.GetConsoleOutput()));
        Assert.Equal($"73894759230\n", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task help_into_should_start_with_provided_text()
    {
        consoleStreams.Reset();

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .Configure((ctx, _, _) => ctx.HelpIntro = "890374586")
            .RunAsync(["help"]);

        Assert.StartsWith("890374586", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task throws_exception_when_default_command_is_not_set_and_command_not_found()
    {
        consoleStreams.Reset();

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .Configure((ctx, _, _) => ctx.DefaultCommand = null)
            .RunAsync(["simple"]);

        Assert.Equal($"{SimpleCommand.ExpectedOutput}\n", consoleStreams.GetConsoleOutput());

        consoleStreams.Reset();

        await Assert.ThrowsAsync<CommandNotFoundException>(async () => await new ECFHostBuilder()
            .UseDefaultCommands()
            .Configure((ctx, _, _) => ctx.DefaultCommand = null)
            .RunAsync(["not-a-real-command"]));
    }

    [Fact]
    public async Task command_alias_binding()
    {
        consoleStreams.Reset();

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync(["aliaso"]);

        Assert.Equal($"{CommandWithAliases.ExpectedOutput}\n", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task different_commands_executed_in_order()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("commandoo");
        consoleStreams.UserInput("command-with-aliases");
        consoleStreams.UserInput("simple");
        consoleStreams.UserInput("raw-dog");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", new string[]
        {
            CommandWithAliases.ExpectedOutput,
            CommandWithAliases.ExpectedOutput,
            SimpleCommand.ExpectedOutput,
            RawCommandWithoutBase.ExpectedOutput,
        });

        Assert.Equal($"{expectedOutput}\n", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task null_set_in_context_should_not_throw()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("simple");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .Configure((ctx, _, _) =>
            {
                ctx.HelpIntro = null;
                ctx.DefaultCommand = null;
                ctx.Intro = null;
                ctx.Prefix = null;
                ctx.CommandProcessor = null;
            })
            .RunAsync([]);

        Assert.Equal($"{SimpleCommand.ExpectedOutput}\n", consoleStreams.GetConsoleOutput());
    }
}
