namespace ECF
{
    public interface ICommand
    {
        void ApplyArguments(CommandArguments args);
        void Execute();
    }
}
