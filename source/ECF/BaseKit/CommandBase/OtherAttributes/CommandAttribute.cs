namespace ECF;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute : Attribute, ICommandAttribute
{
    public string Name { get; }
    public string[]? Aliases { get; }

    public CommandAttribute(string name, params string[] aliases)
    {
        Name = name;
        Aliases = aliases;
    }
}
