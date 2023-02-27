using ECF.Engine;

namespace ECF.Commands
{
    [Command("exit")]
    [CmdDescription("Exists the program")]
    public class ExitCommand : CommandBase
    {
        private readonly InterfaceContext interfaceContext;

        public ExitCommand(InterfaceContext interfaceContext)
        {
            this.interfaceContext = interfaceContext;
        }

        public override void Execute()
        {
            interfaceContext.ForceExit = true;
        }
    }
}
