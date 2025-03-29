using ECF.Utilities;
using System.Reflection;

namespace ECF.Engine;

public class CommandLineInterface
{
    private readonly InterfaceContext interfaceContext;

    public CommandLineInterface(InterfaceContext interfaceContext)
    {
        this.interfaceContext = interfaceContext;
    }

    [Obsolete("Method renamed to RunAsync to match more clearly what it does (blocks thread while it runs). This will be removed soon.")]
    public async Task StartAsync(string[] args, CancellationToken cancellationToken = default)
        => await RunAsync(args, cancellationToken);

    public async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        if (interfaceContext.CommandProcessor == null)
            throw new ArgumentException("CommandProcessor is null. Cannot run command line interface without procesor inside InterfaceContext.");

        var cancellationController = new CancellationController(cancellationToken);

        if (args.Length > 0 || interfaceContext.DisablePrompting)
        {
            try
            {
                cancellationController.StartNew();
                await interfaceContext.CommandProcessor.ProcessAsync(args, cancellationController.SingleCommand);
            }
            catch (Exception ex)
            {
                var userExceptionAttr = ex.GetType().GetCustomAttribute<UserExceptionAttribute>();

                if (userExceptionAttr != null)
                {
                    Environment.ExitCode = userExceptionAttr.ExitCode;
                    ColorConsole.WriteLine(ex.Message, userExceptionAttr.ErrorTextColor);
                    return;
                }
                else
                {
                    throw;
                }
            }
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
                    var userExceptionAttr = ex.GetType().GetCustomAttribute<UserExceptionAttribute>();

                    if (userExceptionAttr != null)
                    {
                        ColorConsole.WriteLine(ex.Message, userExceptionAttr.ErrorTextColor);
                    }
                    else
                    {
                        ColorConsole.WriteLine("Unhandled exception has been thrown:", ConsoleColor.Red);
                        ColorConsole.WriteLine(ex.ToString(), ConsoleColor.Red);
                    }
                }
            }

            Environment.ExitCode = 0;
        }
    }
}
