namespace ECF.Exceptions;

public class ECFException : Exception
{
    public ECFException(string message) : base(message) { }
    public ECFException(string message, Exception innerException) : base(message, innerException) { }
}
