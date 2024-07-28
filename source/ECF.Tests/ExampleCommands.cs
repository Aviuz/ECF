namespace ECF.Tests.ExampleCommands;

[Command("command-with-aliases", "aliaso", "commandoo")] // with aliases
public class CommandWithAliases : CommandBase
{
    public override void Execute() => Console.WriteLine("First command executed");
}

[Command("simple")]
public class SimpleCommand : CommandBase
{
    public override void Execute() => Console.WriteLine("Second command executed");
}

public class CommandWithoutAttribute : CommandBase
{
    public override void Execute() => Console.WriteLine("Third command executed");
}

[Command("raw-dog")]
public class RawCommandWithoutBase : ICommand
{
    public void ApplyArguments(CommandArguments args) { }

    public void Execute() => Console.WriteLine("Custom command executed");
}