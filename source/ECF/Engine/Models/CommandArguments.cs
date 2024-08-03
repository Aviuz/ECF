namespace ECF;

public class CommandArguments
{
    /// <summary>
    /// Command name or alias passed by user. It can be null if used as a default command, or used from dispatcher.
    /// </summary>
    public string? CommandName { get; set; }

    /// <summary>
    /// Arguments passed by user (if first argument was not recognized as command name this will included here)
    /// </summary>
    public string[] Arguments { get; set; }

    /// <summary>
    /// This is special flag that is set to true if command was not found and fallback command is being executed
    /// </summary>
    public bool IsFallbackRequested { get; set; }

    public CommandArguments(string? commandName, string[] arguments, bool isFallbackRequested = false)
    {
        CommandName = commandName;
        Arguments = arguments;
        IsFallbackRequested = isFallbackRequested;
    }
}
