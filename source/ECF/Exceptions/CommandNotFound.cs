namespace ECF.Exceptions;

public class CommandNotFoundException : ECFException
{
    public CommandNotFoundException(string[] args)
        : base($"No command found for input '{string.Join(" ", args)}' and no default command was specified.") { }
}
