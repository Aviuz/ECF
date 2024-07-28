namespace ECF.Commands
{
    public class NotFoundCommand : ICommand
    {
        private string? commandName;

        public void ApplyArguments(CommandArguments args)
        {
            commandName = args.CommandName;
        }

        public void Execute()
        {
            Console.WriteLine($"Command not found: {commandName}. Type 'help' to list commands.");
        }
    }
}
