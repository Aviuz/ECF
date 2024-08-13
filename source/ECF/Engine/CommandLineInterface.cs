using ECF.Utilities;

namespace ECF.Engine;

public class CommandLineInterface
{
    private readonly InterfaceContext interfaceContext;

    public CommandLineInterface(InterfaceContext interfaceContext)
    {
        this.interfaceContext = interfaceContext;
    }

    public async Task StartAsync(string[] args, CancellationToken cancellationToken = default)
    {
        if (interfaceContext.CommandProcessor == null)
            throw new ArgumentException("CommandProcessor is null. Cannot run command line interface without procesor inside InterfaceContext.");

        var cancellationController = new CancellationController(cancellationToken);

        if (args.Length > 0 || interfaceContext.DisablePrompting)
        {
            cancellationController.StartNew();
            await interfaceContext.CommandProcessor.ProcessAsync(args, cancellationController.SingleCommand);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(interfaceContext.Intro))
                Console.WriteLine(interfaceContext.Intro);
            while (!interfaceContext.ForceExit && !cancellationController.Root.IsCancellationRequested)
            {
                if (interfaceContext.Prefix != null)
                    Console.Write(interfaceContext.Prefix);

                string? input = await Console.In.ReadLineAsync().WithCancellation(cancellationController.Root);
                
                if (input == null) return; // Ctrl+C was pressed

                try
                {
                    cancellationController.StartNew();
                    await interfaceContext.CommandProcessor.ProcessAsync(input, cancellationController.SingleCommand);
                    cancellationController.Reset();
                }
                catch (Exception ex)
                {
                    ColorConsole.WriteLine("Exception thrown:", ConsoleColor.Red);
                    ColorConsole.WriteLine(ex.ToString(), ConsoleColor.Red);
                }
            }
        }
    }
}
