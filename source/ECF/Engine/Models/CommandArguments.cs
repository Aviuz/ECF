namespace ECF;

public class CommandArguments
{
    /// <summary>
    /// Command name or alias passed by user. It can be null if used as a default command, or used from dispatcher.
    /// </summary>
    public string? CommandName { get; init; }

    /// <summary>
    /// Arguments passed by user (if first argument was not recognized as command name this will included here)
    /// </summary>
    public string[] Arguments { get; init; }

    /// <summary>
    /// Set to true if command was not found and fallback command is being executed, or when no arguments were passed.
    /// </summary>
    public bool ExecutedAsDefaultCommand { get; init; }

    private CommandArguments(string? commandName, string[] arguments)
    {
        CommandName = commandName;
        Arguments = arguments;
    }

    public static CommandArguments Empty() => new CommandArguments(null, new string[0]);
    public static CommandArguments ForMappedCommand(string commandName, string[] arguments) => new CommandArguments(commandName, arguments);
    public static CommandArguments FromCode(string[] arguments) => new CommandArguments(null, arguments);
    public static CommandArguments ForDefaultCommand(string[] arguments) => new CommandArguments(null, arguments) { ExecutedAsDefaultCommand = true };
}
