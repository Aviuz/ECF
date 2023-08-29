namespace ECF.Engine;

public class CommandLineInterface
{
    private readonly InterfaceContext interfaceContext;

    public CommandLineInterface(InterfaceContext interfaceContext)
    {
        this.interfaceContext = interfaceContext;
    }

    public void Start(string[] args)
    {
        if (interfaceContext.CommandProcessor == null)
            throw new ArgumentException("CommandProcessor is null. Cannot run command line interface without procesor inside InterfaceContext.");

        if (args.Length > 0)
        {
            interfaceContext.SilentMode = true;
            interfaceContext.CommandProcessor.Process(args);
        }
        else
        {
            if (!string.IsNullOrWhiteSpace(interfaceContext.Intro))
                Console.WriteLine(interfaceContext.Intro);
            while (!interfaceContext.ForceExit)
            {
                Console.Write(interfaceContext.Prefix + " ");
                string? input = Console.ReadLine();
                try
                {
                    interfaceContext.CommandProcessor.Process(input);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
    }
}
