using System.Diagnostics;

namespace ECF.Utilities;

public class Spinner : IDisposable
{
    private static readonly char[] Sequence = { '\\', '|', '/', '-' };
    private int counter;
    private int strLength;
    private bool isLoading;
    private Timer? spinnerTimer;
    private Stopwatch stopwatch = new Stopwatch();

    public bool IsLoading
    {
        get { return isLoading; }
        set
        {
            lock (this)
            {
                if (value != isLoading)
                {
                    isLoading = value;
                    if (isLoading)
                    {
                        Console.CursorVisible = false;
                        Start();
                    }
                    else
                    {
                        Stop();
                        Console.CursorVisible = true;
                    }
                }
            }
        }
    }

    private void Start()
    {
        stopwatch.Start();
        spinnerTimer = new Timer(Spin, null, 0, 100);
    }

    private void Stop()
    {
        stopwatch.Stop();
        spinnerTimer?.Change(Timeout.Infinite, Timeout.Infinite);
        Erase();
    }

    private void Spin(object? state)
    {
        lock (this)
        {
            if (!isLoading) return;
            Console.CursorLeft = 0;
            string displayStr = $"{Sequence[counter % Sequence.Length]} {FormatElapsedTime(stopwatch.Elapsed)}";
            strLength = displayStr.Length;
            Console.Write(displayStr);
            counter++;
        }
    }

    private void Erase()
    {
        Console.CursorLeft = 0;
        Console.WriteLine(new string(' ', strLength));
        Console.CursorLeft = 0;
    }

    private string FormatElapsedTime(TimeSpan elapsed)
    {
        if (elapsed.TotalHours >= 1)
            return $"{(int)elapsed.TotalHours}h {elapsed.Minutes}m";
        if (elapsed.TotalMinutes >= 1)
            return $"{elapsed.Minutes}m {elapsed.Seconds}s";
        return $"{elapsed.Seconds}s";
    }

    public void Dispose()
    {
        spinnerTimer?.Dispose();
    }
}
