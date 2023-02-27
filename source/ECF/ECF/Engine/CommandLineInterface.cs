using System;

namespace ECF.Engine
{
    public class CommandLineInterface
    {
        private readonly InterfaceContext interfaceContext;

        public CommandLineInterface(InterfaceContext interfaceContext)
        {
            this.interfaceContext = interfaceContext;
        }

        public void Start(string[] args)
        {
            if (interfaceContext.CommandScope == null)
                throw new ArgumentException("Cannot run command line interface without command scope.");

            if(interfaceContext.CommandScope.Processor == null)
                throw new ArgumentException("Cannot run command line interface without procesor inside CommandScope. If you created your own CommandScope be sure to create ICommandProcesor and assign to Processor property.");

            if (args.Length > 0)
            {
                interfaceContext.SilentMode = true;
                interfaceContext.CommandScope.Processor.Process(args);
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
                        interfaceContext.CommandScope.Processor.Process(input);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }
    }
}
