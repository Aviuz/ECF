using Autofac;
using ECF.Engine;
using ECF.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ECF
{
    public class ECFProgramBuilder
    {
        public ContainerBuilder ContainerBuilder { get; }
        public InterfaceContext InterfaceContext { get; }
        public CommandRegistryBuilder ECFBuilder { get; }

        public ECFProgramBuilder()
        {
            ContainerBuilder = new ContainerBuilder();
            InterfaceContext = new InterfaceContext();
            ECFBuilder = new CommandRegistryBuilder(ContainerBuilder, InterfaceContext);
        }

        public ECFProgramBuilder UseDefaultCommands()
        {
            try
            {
                ECFBuilder.RegisterCommands<CommandAttribute>(Assembly.GetExecutingAssembly());
                ECFBuilder.RegisterCommands<CommandAttribute>(Assembly.GetCallingAssembly());
                ECFBuilder.RegisterCommands<CommandAttribute>(Assembly.GetEntryAssembly()!);
            }
            catch (Exception ex)
            {
                throw new ECFInitializationException(ex);
            }

            return this;
        }

        public ECFProgramBuilder Configure(Action<InterfaceContext, ContainerBuilder, CommandRegistryBuilder> configureAction)
        {
            try
            {
                configureAction(InterfaceContext, ContainerBuilder, ECFBuilder);
            }
            catch (Exception ex)
            {
                throw new ECFInitializationException(ex);
            }

            return this;
        }

        public ECFProgramBuilder AddConfiguration(string configurationFileName = "appsettings.json")
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configurationFileName, false, true)
                .Build();

            ContainerBuilder.RegisterInstance(configuration);

            return this;
        }

        public void Run(string[] args)
        {
            if (InterfaceContext.CommandScope == null)
                InterfaceContext.CommandScope = new CommandScope(ContainerBuilder.Build());

            if (InterfaceContext.CommandScope == null)
                throw new ArgumentException("There were no CommandScope initialized. Please use Configure(Action<InterfaceContext, ContainerBuilder>) method first, before starting program.");

            CommandLineInterface commandInterface = new(InterfaceContext);

            commandInterface.Start(args);
        }
    }
}
