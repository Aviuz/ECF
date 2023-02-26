using Autofac;
using ECF.Engine;
using ECF.Exceptions;

namespace ECF
{
    public class ECFConsoleProgram
    {
        public InterfaceContext Context { get; } = new InterfaceContext();

        public ECFConsoleProgram Configure(Action<InterfaceContext, ContainerBuilder> configureAction)
        {
            var builder = new ContainerBuilder();

            try
            {
                builder.RegisterInstance(Context);
                builder.RegisterCommands<CommandAttribute>();

                configureAction(Context, builder);

                Context.CommandScope = new CommandScope(builder.Build());
            }
            catch (Exception ex)
            {
                throw new ContainerException(ex);
            }

            return this;
        }

        public ECFConsoleProgram UseScope(Func<InterfaceContext, CommandScope> scopeFunction)
        {
            Context.CommandScope = scopeFunction(Context);

            return this;
        }

        public void Start(string[] args)
        {
            if (Context.CommandScope == null)
                throw new ArgumentException("There were no CommandScope initialized. Please use Configure(Action<InterfaceContext, ContainerBuilder>) method first, before starting program.");

            CommandLineInterface commandInterface = new(Context);

            commandInterface.Start(args);
        }
    }
}
