using ECF.Engine;
using ECF.Utilities;

namespace ECF.BaseKitCommands;

[Command("load-script")]
[CmdSyntax("<FilePath>")]
[CmdDescription("runs script from file")]
[CmdArgument("filepath", 0, Description = "loads script from file specified in <FilePath>")]
public class LoadScriptCommand : AsyncCommandBase
{
    private readonly InterfaceContext interfaceContext;

    string? FilePath => GetValue("filepath");

    public LoadScriptCommand(InterfaceContext interfaceContext)
    {
        this.interfaceContext = interfaceContext;
    }

    public override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(FilePath))
        {
            ColorConsole.WriteLine("You need to provide file path to script file", ConsoleColor.Red);
            DisplayHelp();
            return;
        }

        ScriptLoader loader = new(interfaceContext);

        using (var fileSteram = File.OpenRead(FilePath))
        using (var textReader = new StreamReader(fileSteram))
        {
            await loader.LoadAsync(textReader, cancellationToken);
        }
    }
}
