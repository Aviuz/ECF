namespace ECF.Exceptions
{
    public class ContainerException : Exception
    {
        private static string ExceptionMessage = "There was problem during container initialization. See inner exception for details.";

        public ContainerException(Exception innerException) : base(ExceptionMessage, innerException)
        {
        }
    }
}
