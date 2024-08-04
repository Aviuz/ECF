using ECF.Engine;
using ECF.Tests.HighLevelTextTests.HighLevelTextTests;
using ECF.Tests.Mocks.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace ECF.Tests.HighLevelTextTests;

[Collection("ConsoleTests")]
public class UsingMicrosoftServices
{
    private readonly ConsoleSteamsFixture consoleStreams;

    public UsingMicrosoftServices(ConsoleSteamsFixture consoleStreams)
    {
        this.consoleStreams = consoleStreams;
    }

    [Fact]
    public async Task help_into_should_start_with_provided_text()
    {
        consoleStreams.Reset();

        var commandline = PrepareDefaultCommandLineUsingCustomConfiguration(ctx => ctx.HelpIntro = "5284749012");
        await commandline.StartAsync(["help"]);

        Assert.StartsWith("5284749012", consoleStreams.GetConsoleOutput());
    }

    public static CommandLineInterface PrepareDefaultCommandLineUsingCustomConfiguration(Action<InterfaceContext> configure)
    {
        var interfaceContext = new InterfaceContext();

        interfaceContext.DefaultCommand = typeof(BaseKitCommands.HelpCommand);

        interfaceContext.CommandProcessor = new ServiceCollection()
            .AddSingleton(interfaceContext)
            .AddECFCommandRegistry(x => x
                .RegisterCommands<CommandAttribute>(typeof(BaseKitCommands.HelpCommand).Assembly)
                .RegisterCommands<CommandAttribute>(typeof(SimpleCommand).Assembly))
            .BuildAndCreateECFCommandProcessor();

        configure(interfaceContext);

        return new CommandLineInterface(interfaceContext);
    }
}
