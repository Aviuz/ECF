namespace ECF.Engine
{
    public class InterfaceContext
    {
        public bool ForceExit { get; set; }

        /// <summary>
        /// If set to true it will run command in background, without user interface
        /// </summary>
        public bool SilentMode { get; set; }

        /// <summary>
        /// Symbol or string that will be displayed when user is prompted to enter command
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Intro text that will be displayed when program starts.
        /// </summary>
        public string? Intro { get; set; }

        /// <summary>
        /// Intro text for help command that will be displayed when help command executes.
        /// </summary>
        public string? HelpIntro { get; set; }

        /// <summary>
        /// If set to not to null it will redirect all input to specified interface.
        /// It only works in silent mode and in load-script command.
        /// </summary>
        public ICommandProcessor? CommandProcessor { get; set; }
    }
}
