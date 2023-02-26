using ECF.Engine;

namespace ECF.Commands
{
    [Command("exit")]
    public class ExitCommand : ICommand
    {
        private readonly InterfaceContext interfaceContext;

        public ExitCommand(InterfaceContext interfaceContext)
        {
            this.interfaceContext = interfaceContext;
        }

        public void ApplyArguments(CommandArguments args) { }

        public void Execute()
        {
            interfaceContext.ForceExit = true;
        }
    }
}
