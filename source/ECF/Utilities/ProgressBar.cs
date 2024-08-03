namespace ECF.Utilities;

public class ProgressBar
{
    private const int Length = 25;

    private int progress;
    private bool isLoading;

    public int Progress
    {
        get { return progress; }
        set
        {
            if (progress != value)
            {
                if (value < 0)
                    value = 0;

                if (value > 100)
                    value = 100;

                progress = value;

                if (IsLoading)
                    UpdateProgress();
            }
        }
    }

    public bool IsLoading
    {
        get { return isLoading; }
        set
        {
            if (value != isLoading)
            {
                if (value)
                    Enable();
                else
                    Disable();
                isLoading = value;
            }
        }
    }

    /// <summary>
    /// Works similiar to Console.WriteLine but places message above loading bar if it is visible
    /// </summary>
    public void WriteLine(string message)
    {
        if (isLoading)
        {
            IsLoading = false;
            Console.WriteLine(message);
            IsLoading = true;
        }
        else
        {
            Console.WriteLine(message);
        }
    }

    private void UpdateProgress()
    {
        lock (this)
        {
            int filled = Progress * Length / 100;

            Console.CursorLeft = 0;

            Console.Write("│");
            Console.Write(new string('█', filled));
            Console.Write(new string('░', Length - filled));
            Console.Write("│");

            Console.Write($" {Progress}%");

            Console.CursorLeft = 0;
        }
    }

    private void Erase()
    {
        Console.CursorLeft = 0;

        Console.Write(new string(' ', Length + 7));

        Console.CursorLeft = 0;
    }

    private void Enable()
    {
        Console.CursorVisible = false;
        UpdateProgress();
    }

    private void Disable()
    {
        Erase();

        Console.CursorVisible = true;
    }
}
