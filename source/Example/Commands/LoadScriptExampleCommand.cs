using ECF;
using ECF.Engine;
using ECF.Utilities;
using Example.Properties;

namespace Example.Commands
{
    [Command("load-example")]
    [CmdDescription("loads example script from resources")]
    public class LoadScriptExampleCommand : CommandBase
    {
        private readonly InterfaceContext interfaceContext;

        public LoadScriptExampleCommand(InterfaceContext interfaceContext)
        {
            this.interfaceContext = interfaceContext;
        }

        public override void Execute()
        {
            ColorConsole.WriteLine($"loading scripts from {nameof(Resources.ExampleScript)}...", ConsoleColor.DarkGray);
            ColorConsole.WriteLine(Resources.ExampleScript, ConsoleColor.DarkGray);

            var loader = new ScriptLoader(interfaceContext);

            using (var reader = new StringReader(Resources.ExampleScript))
            {
                loader.Load(reader);
            }
        }
    }
}
