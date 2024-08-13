using ECF.Exceptions;
using ECF.Tests.HighLevelTextTests.HighLevelTextTests;
using ECF.Tests.Mocks.Commands;
using System.Reflection;
using System.Text;

namespace ECF.Tests.HighLevelTextTests;

[Collection("ConsoleTests")]
public class SingleCommandTests
{
    private readonly ConsoleSteamsFixture consoleStreams;

    public SingleCommandTests(ConsoleSteamsFixture consoleStreams)
    {
        this.consoleStreams = consoleStreams;
    }

    [Fact]
    public async Task single_command_mode_works_in_happy_path()
    {
        consoleStreams.Reset();

        await new ECFHostBuilder()
                .UseSingleCommand<CommandWithOneParameter>()
                .RunAsync("-p 72635491213".Tokenize());

        Assert.Equal("Parameter:72635491213\n", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task single_command_mode_works_in_happy_path_for_arguments_too()
    {
        consoleStreams.Reset();

        await new ECFHostBuilder()
                .UseSingleCommand<CommandWithOneArgument>()
                .RunAsync(["72635491213"]);

        Assert.Equal("Argument:72635491213\n", consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task prompt_mode_is_disabled_when_single_command_mode()
    {
        consoleStreams.Reset();

        var cancel = new CancellationTokenSource();

        var hostTask = new ECFHostBuilder()
                .UseSingleCommand<SimpleCommand>()
                .Configure((ctx, _, _) =>
                {
                    ctx.Intro = "73894759230";
                })
                .RunAsync([], cancel.Token);

        await Task.WhenAny(hostTask, Task.Delay(100).ContinueWith((_) => cancel.Cancel())); // add little delay for host to start and exit immadiately after

        Assert.True(hostTask.IsCompleted, "host should exit immadiately when prompt mode is disabled");
    }

    [Fact]
    public async Task throws_if_single_command_register_default_comamnds()
    {
        await Assert.ThrowsAsync<ECFCommandRegistryNotEmptyWhenUsingSingleMode>(async () =>
        {
            await new ECFHostBuilder()
                .UseSingleCommand<SimpleCommand>()
                .UseDefaultCommands()
                .Configure((ctx, _, _) =>
                {
                    ctx.HelpIntro = null;
                    ctx.DefaultCommand = null;
                    ctx.Intro = null;
                    ctx.Prefix = null;
                    ctx.CommandProcessor = null;
                })
                .RunAsync(["exit"]);
        });
    }

    [Fact]
    public async Task throws_if_single_command_register_commands_by_attribute()
    {
        await Assert.ThrowsAsync<ECFCommandRegistryNotEmptyWhenUsingSingleMode>(async () =>
        {
            await new ECFHostBuilder()
                .UseSingleCommand<SimpleCommand>()
                .Configure((ctx, _, registry) =>
                {
                    registry.RegisterCommands<CommandAttribute>(Assembly.GetExecutingAssembly());
                })
                .RunAsync(["exit"]);
        });
    }
}
