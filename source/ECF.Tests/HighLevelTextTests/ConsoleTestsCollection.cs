using System.Text;

namespace ECF.Tests.HighLevelTextTests.HighLevelTextTests;


[CollectionDefinition("ConsoleTests")]
public class ConsoleTestsCollection : ICollectionFixture<ConsoleSteamsFixture>
{
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
}

public class ConsoleSteamsFixture : IDisposable
{
    public ConsoleSteamsFixture()
    {
        InMemoryStream = new InputSteamMock(new MemoryStream());
        InMemoryReader = TextReader.Synchronized(new StreamReader(InMemoryStream, Encoding.UTF8));
        Console.SetIn(InMemoryReader);

        OutMemoryStream = new();
        OutMemoryWriter = new(OutMemoryStream, Encoding.UTF8);
        Console.SetOut(OutMemoryWriter);
    }

    public void Reset()
    {
        OutMemoryWriter.Flush();

        InMemoryStream.SetLength(0);
        OutMemoryStream.SetLength(0);
        InMemoryStream.ResetSemaphore();
    }

    public InputSteamMock InMemoryStream { get; }
    public TextReader InMemoryReader { get; }

    public MemoryStream OutMemoryStream { get; }
    public StreamWriter OutMemoryWriter { get; }

    public void UserInput(string input)
    {
        foreach (var line in input.Split('\n'))
            InMemoryStream.Process(line);
    }

    public string GetConsoleOutput()
    {
        OutMemoryWriter.Flush();
        string str = Encoding.UTF8.GetString(OutMemoryStream.ToArray());

        if (str.Length > 0 && str[0] == 0xFEFF)
            str = str.Substring(1); // Remove BOM (Byte Order Mark)

        return str
            .Replace("\r\n", "\n"); // Normalize line endings (Windows to Unix)
    }

    public void Dispose()
    {
        InMemoryReader.Dispose();
        InMemoryStream.Dispose();
        OutMemoryWriter.Dispose();
        OutMemoryStream.Dispose();
    }
}

public class InputSteamMock : MemoryStream
{
    private readonly MemoryStream innerStream;
    private Semaphore semaphore = new(0, 100);

    public InputSteamMock(MemoryStream innerStream)
    {
        this.innerStream = innerStream;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        semaphore.WaitOne();
        return innerStream.Read(buffer, offset, count);
    }

    public void Process(string str)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(str + '\n');

        long currentPosition = innerStream.Position;

        innerStream.Seek(0, SeekOrigin.End);
        innerStream.Write(bytes);
        innerStream.Position = currentPosition;
        semaphore.Release(1);
    }

    public void ResetSemaphore()
    {
        semaphore = new(0, 100);
    }
}