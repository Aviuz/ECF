namespace ECF.Exceptions;

public class MissingCommandInterfaceException : ECFException
{
    public MissingCommandInterfaceException(Type commandType)
        : base($"Command '{commandType.FullName}' need to implement {nameof(ICommand)} interface")
    { }
}
