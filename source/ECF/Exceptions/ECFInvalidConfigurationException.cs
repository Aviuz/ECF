namespace ECF.Exceptions;

public class ECFInvalidConfigurationException : ECFException
{
    private static string ExceptionMessageFormat = "Invalid configuration of ECF: {0}";

    public ECFInvalidConfigurationException(string message) : base(string.Format(ExceptionMessageFormat, message)) { }
}
