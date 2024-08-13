using ECF.Utilities;

namespace ECF.Engine;

internal class CancellationController
{
    private readonly CancellationTokenSource rootSource;
    private CancellationTokenSource? singleCommandSource;

    public CancellationToken Root => rootSource.Token;
    public CancellationToken SingleCommand => singleCommandSource?.Token ?? default;

    public CancellationController(CancellationToken parentToken)
    {
        rootSource = CancellationTokenSource.CreateLinkedTokenSource(parentToken);

        Console.CancelKeyPress += Console_CancelKeyPress;
    }

    public void StartNew() => singleCommandSource = CancellationTokenSource.CreateLinkedTokenSource(Root);

    public void Reset() => singleCommandSource = null;

    private void Console_CancelKeyPress(object? sender, ConsoleCancelEventArgs e)
    {
        if (singleCommandSource == null)
        {
            e.Cancel = true; // Prevent the process from terminating immediately
            rootSource.Cancel();
        }
        else if (singleCommandSource != null && singleCommandSource.IsCancellationRequested == false)
        {
            e.Cancel = true; // Prevent the process from terminating immediately
            singleCommandSource.Cancel();
            ColorConsole.WriteLine("Cancellation requested. Press again ctrl+c to force shutdown.", ConsoleColor.Yellow);
        }
        else // this is probably second time ctrl+c was pressed
        {
            rootSource.Cancel();
        }
    }
}
