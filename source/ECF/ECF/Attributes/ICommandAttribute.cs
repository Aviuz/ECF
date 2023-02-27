namespace ECF
{
    public interface ICommandAttribute
    {
        string[]? Aliases { get; }
        string Name { get; set; }
    }
}
