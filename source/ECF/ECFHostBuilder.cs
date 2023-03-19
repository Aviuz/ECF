using Autofac;
using ECF.Engine;
using ECF.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ECF
{
    public class ECFHostBuilder
    {
        public ContainerBuilder ContainerBuilder { get; }
        public InterfaceContext InterfaceContext { get; }
        public CommandRegistryBuilder RegistryBuilder { get; }

        public ECFHostBuilder()
        {
            ContainerBuilder = new ContainerBuilder();
            InterfaceContext = new InterfaceContext();
            RegistryBuilder = new CommandRegistryBuilder(InterfaceContext, ContainerBuilder);
        }

        public ECFHostBuilder UseDefaultCommands()
        {
            try
            {
                RegistryBuilder.RegisterCommands<CommandAttribute>(Assembly.GetExecutingAssembly());
                RegistryBuilder.RegisterCommands<CommandAttribute>(Assembly.GetCallingAssembly());
                RegistryBuilder.RegisterCommands<CommandAttribute>(Assembly.GetEntryAssembly()!);
            }
            catch (Exception ex)
            {
                throw new ECFInitializationException(ex);
            }

            return this;
        }

        public ECFHostBuilder Configure(Action<InterfaceContext, ContainerBuilder, CommandRegistryBuilder> configureAction)
        {
            try
            {
                configureAction(InterfaceContext, ContainerBuilder, RegistryBuilder);
            }
            catch (Exception ex)
            {
                throw new ECFInitializationException(ex);
            }

            return this;
        }

        public ECFHostBuilder AddConfiguration(string configurationFileName = "appsettings.json", bool optional = false, bool reloadOnChange = true)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(configurationFileName, optional, reloadOnChange)
                .Build();

            ContainerBuilder.RegisterInstance<IConfiguration>(configuration);

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
