namespace ECF
{
    public class CommandArguments
    {
        public string CommandName { get; set; }
        public string[] Arguments { get; set; }

        public CommandArguments(string commandName, string[] arguments)
        {
            CommandName = commandName;
            Arguments = arguments;
        }
    }
}
