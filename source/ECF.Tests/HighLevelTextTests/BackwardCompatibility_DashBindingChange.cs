using ECF.Tests.HighLevelTextTests.HighLevelTextTests;

namespace ECF.Tests.HighLevelTextTests;

[Collection("ConsoleTests")]
public class BackwardCompatibility_DashBindingChange
{
    private readonly ConsoleSteamsFixture consoleStreams;

    public BackwardCompatibility_DashBindingChange(ConsoleSteamsFixture consoleStreams)
    {
        this.consoleStreams = consoleStreams;
    }

    [Fact]
    public async Task old_parameters_binding_working_as_intented()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("old-parameters-binding -s \"My short value\"");
        consoleStreams.UserInput("old-parameters-binding --long \"My long value\"");
        consoleStreams.UserInput("old-parameters-binding -m \"My mixed value\"");
        consoleStreams.UserInput("old-parameters-binding --mixed \"My mixed value\"");
        consoleStreams.UserInput("old-parameters-binding -s All --long aboard -m kuka --mixed ship");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", [
            "My short value",
            "My long value",
            "My mixed value",
            "My mixed value",
            "Allaboardship"
        ]) + '\n';

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task old_flags_binding_working_as_intented()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("old-flags-binding -s");
        consoleStreams.UserInput("old-flags-binding --long");
        consoleStreams.UserInput("old-flags-binding -m");
        consoleStreams.UserInput("old-flags-binding --mixed");
        consoleStreams.UserInput("old-flags-binding -s --long aboard -m --mixed ship");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", [
            "Short:True|Long:False|Mixed:False",
            "Short:False|Long:True|Mixed:False",
            "Short:False|Long:False|Mixed:True",
            "Short:False|Long:False|Mixed:True",
            "Short:True|Long:True|Mixed:True"
        ]) + '\n';

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task new_parameters_binding_working_as_intented()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("new-parameters-binding -m \"My short value\"");
        consoleStreams.UserInput("new-parameters-binding --mixed \"My long value\"");
        consoleStreams.UserInput("new-parameters-binding ~mixed \"My weird value\"");
        consoleStreams.UserInput("new-parameters-binding -m \"should not display\" --mixed ✔");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", [
            "My short value",
            "My long value",
            "My weird value",
            "✔"
        ]) + '\n';

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }

    [Fact]
    public async Task new_flags_binding_working_as_intented()
    {
        consoleStreams.Reset();

        consoleStreams.UserInput("new-flags-binding -m ");
        consoleStreams.UserInput("new-flags-binding --mixed ");
        consoleStreams.UserInput("new-flags-binding ~mixed ");
        consoleStreams.UserInput("new-flags-binding -m --mixed");
        consoleStreams.UserInput("new-flags-binding");
        consoleStreams.UserInput("exit");

        await new ECFHostBuilder()
            .UseDefaultCommands()
            .RunAsync([]);

        string expectedOutput = string.Join("\n", [
            "True",
            "True",
            "True",
            "True",
            "False"
        ]) + '\n';

        Assert.Equal(expectedOutput, consoleStreams.GetConsoleOutput());
    }
}
