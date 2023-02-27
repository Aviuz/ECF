using ECF.Engine;
using ECF.Utilities;

namespace ECF.Commands
{
    [Command("load-script")]
    [CmdSyntax("<FilePath>")]
    [CmdDescription("runs script from file")]
    [CmdArg("filepath", 0, Description = "loads script from file specified in <FilePath>")]
    public class LoadScriptCommand : CommandBase
    {
        private readonly InterfaceContext interfaceContext;

        string? FilePath => GetValue("filepath");

        public LoadScriptCommand(InterfaceContext interfaceContext)
        {
            this.interfaceContext = interfaceContext;
        }

        public override void Execute()
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                throw new NotImplementedException();

            ScriptLoader loader = new(interfaceContext);

            using (var fileSteram = File.OpenRead(FilePath))
            using (var textReader = new StreamReader(fileSteram))
            {
                loader.Load(textReader);
            }
        }
    }
}
