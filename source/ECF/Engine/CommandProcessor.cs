using Autofac;
using System.Runtime.InteropServices;

namespace ECF.Engine
{
    public interface ICommandProcesor
    {
        void Process(string? commandLine);
        void Process(string[] args);
    }

    public class CommandProcessor : ICommandProcesor
    {
        [DllImport("shell32.dll", SetLastError = true)]
        static extern IntPtr CommandLineToArgvW([MarshalAs(UnmanagedType.LPWStr)] string? lpCmdLine, out int pNumArgs);

        private readonly IContainer container;

        public CommandProcessor(IContainer container)
        {
            this.container = container;
        }

        public void Process(string? commandLine) => Process(CommandLineToArgs(commandLine));

        public void Process(string[] args)
        {
            using (var scope = container.BeginLifetimeScope())
            {
                var resolver = scope.Resolve<ICommandResolver>();

                ICommand command = resolver.CreateCommand(ParseArguments(args));

                command.Execute();
            }
        }

        private CommandArguments ParseArguments(string[] args)
        {
            return new CommandArguments()
            {
                CommandName = args[0],
                Arguments = args.Skip(1).ToArray(),
            };
        }

        private string[] CommandLineToArgs(string? commandLine)
        {
            int argc;
            var argv = CommandLineToArgvW(commandLine, out argc);

            if (argv == IntPtr.Zero)
            {
                throw new System.ComponentModel.Win32Exception();
            }

            try
            {
                var args = new string[argc];
                for (int i = 0; i < args.Length; i++)
                {
                    var p = Marshal.ReadIntPtr(argv, i * IntPtr.Size);
                    args[i] = Marshal.PtrToStringUni(p)!;
                }

                return args;
            }
            finally
            {
                Marshal.FreeHGlobal(argv);
            }
        }
    }
}
