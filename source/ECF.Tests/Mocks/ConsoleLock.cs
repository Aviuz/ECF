namespace ECF.Tests.Mocks;

internal class ConsoleLock
{
    public static object LockObject { get; } = new object();
}
