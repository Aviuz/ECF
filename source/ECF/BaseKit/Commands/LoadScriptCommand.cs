using ECF.Engine;
using ECF.Utilities;

namespace ECF.BaseKitCommands;

[Command("load-script")]
[CmdSyntax("<file_path>")]
[CmdDescription("runs script from file")]
public class LoadScriptCommand : AsyncCommandBase
{
    private readonly InterfaceContext interfaceContext;

    [Argument(0, Name = "file_path", Description = "loads script from file specified in <file_path>")]
    public string? FilePath { get; set; }

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
