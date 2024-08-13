using ECF;
using ECF.Utilities;

namespace Example.Commands;

[Command("synchronous-test-progress", "sync-progress")]
public class TestProgressBar_Synchronous : CommandBase
{
    [Flag("-t --tens", Description = "write every tens to console")]
    public bool WriteEveryTens { get; set; }

    public override void Execute()
    {
        var progressBar = new ProgressBar();

        Console.WriteLine("Let's load some very large data...");
        progressBar.IsLoading = true; // this will display progress bar inside console window

        var backgroundTask = BackgroundTask(progressBar, default);

        for (int i = 0; i < 100; i++)
        {
            Thread.Sleep(100);

            progressBar.Progress++;

            if (i % 10 == 0 && WriteEveryTens)
            {
                progressBar.IsLoading = false;
                Console.WriteLine($"It's {i}"); // keep in mind this can actaully print over BackgroundTask, but that's on you 😉
                progressBar.IsLoading = true;
            }
        }

        progressBar.IsLoading = false; // this will hide progress bar

        Console.WriteLine("Finished!");

        backgroundTask.Wait();
    }

    public async Task BackgroundTask(ProgressBar progressBar, CancellationToken cancellationToken)
    {
        await Task.Delay(300, cancellationToken);

        if (cancellationToken.IsCancellationRequested)
            return;

        progressBar.IsLoading = false; // this is thread safe operation and will enable clear console to print messages
        ColorConsole.WriteLine("AsyncTest", ConsoleColor.Magenta); // this will print on top of progress bar if you do not disable it, you can comment line above to see the difference
        ColorConsole.WriteLine(@"This is entry is from asynchonous background task. Progress bar should be thread safe if disabled before printing anything.", ConsoleColor.Magenta);
        progressBar.IsLoading = true; // put this back on
    }
}