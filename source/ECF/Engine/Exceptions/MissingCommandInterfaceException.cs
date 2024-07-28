namespace ECF.Engine.Exceptions;

public class MissingCommandInterfaceException : Exception
{
    public MissingCommandInterfaceException(Type commandType)
        : base($"Command '{commandType.FullName}' need to implement {nameof(ICommand)} interface")
    { }
}
