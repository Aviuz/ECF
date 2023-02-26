using ECF;
using ECF.Utilities;

namespace Example.Commands
{
    [Command("test-progress")]
    [CmdFlag("write")]
    public class TestProgressBar : CommandBase
    {
        public override void Execute()
        {
            var progressBar = new ProgressBar();

            Console.WriteLine("testing ...");

            progressBar.IsLoading = true;

            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                progressBar.Progress++;
                if (i == 33 && IsFlagActive("write"))
                {
                    Console.WriteLine("It's 33");
                }
            }

            progressBar.IsLoading = false;

            Console.WriteLine("finished ...");
        }
    }
}
