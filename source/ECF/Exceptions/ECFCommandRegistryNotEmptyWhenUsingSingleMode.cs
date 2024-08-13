namespace ECF.Exceptions;

public class ECFCommandRegistryNotEmptyWhenUsingSingleMode : ECFInvalidConfigurationException
{
    public ECFCommandRegistryNotEmptyWhenUsingSingleMode() : base("When using UseSingleCommand, CommandRegistry should be empty. That means you shouldn't register any command by Register(...) or by AddDefaultCommands()") { }
}