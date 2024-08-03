namespace ECF.Engine;

public class InterfaceContext
{
    public bool ForceExit { get; set; }

    /// <summary>
    /// If set to true it will try to run default command instead of prompting user for input when run without arguments.
    /// </summary>
    public bool DisablePrompting { get; set; }

    /// <summary>
    /// Symbol or string that will be displayed when user is prompted to enter command
    /// </summary>
    public string? Prefix { get; set; }

    /// <summary>
    /// Intro text that will be displayed when program starts without arguments. It won't show up if DisablePrompting is set to true.
    /// </summary>
    public string? Intro { get; set; }

    /// <summary>
    /// Intro text for help command that will be displayed when help command executes. Only works when using HelpCommand from basekit.
    /// </summary>
    public string? HelpIntro { get; set; }

    /// <summary>
    /// If set to not to null it will redirect all input to specified interface.
    /// It only works in silent mode and in load-script command.
    /// </summary>
    public ICommandProcessor? CommandProcessor { get; set; }

    /// <summary>
    /// This command will be executed if no arguments are passed or command was found by mathing first argument.
    /// </summary>
    public Type? DefaultCommand { get; set; }
}
