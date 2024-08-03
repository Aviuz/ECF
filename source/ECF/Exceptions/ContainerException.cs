namespace ECF.Exceptions;

public class ECFInitializationException : ECFException
{
    private static string ExceptionMessage = "There was problem during ECF initialization. See inner exception for details.";

    public ECFInitializationException(Exception innerException) : base(ExceptionMessage, innerException) { }
}
