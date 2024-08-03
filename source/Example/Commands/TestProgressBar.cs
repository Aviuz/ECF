using ECF;
using ECF.Utilities;

namespace Example.Commands;

[Command("test-progress")]
public class TestProgressBar : CommandBase
{
    [Flag("-t --tens", Description = "write every tens to console")]
    public bool WriteEveryTens { get; set; }

    public override void Execute()
    {
        var progressBar = new ProgressBar();

        Console.WriteLine("testing ...");

        progressBar.IsLoading = true;

        Task.Run(async () =>
        {
            await Task.Delay(300);

            lock (progressBar)
            {
                const string message =
@"This is entry is from asynchonous. 
Remember to lock on ProgressBar object.
This will ensure it won't interfere with prograss bar render in console.";

                Console.WriteLine(message);
            }
        });

        for (int i = 0; i < 100; i++)
        {
            Thread.Sleep(100);
            progressBar.Progress++;
            if (i % 10 == 0 && WriteEveryTens)
            {
                Console.WriteLine($"It's {i}");
            }
        }

        progressBar.IsLoading = false;

        Console.WriteLine("finished ...");
    }
}
