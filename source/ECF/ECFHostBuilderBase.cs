using ECF.Engine;
using ECF.Exceptions;
using ECF.InverseOfControl;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ECF;

public class ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> where TDPBuilderAdapter : class, IIoCBuilderAdapter<TDPBuilder> where TDPBuilder : class
{
    public TDPBuilderAdapter IoCBuilderAdapter { get; }
    public InterfaceContext InterfaceContext { get; }
    public CommandRegistryBuilder RegistryBuilder { get; }

    public ECFHostBuilderBase(TDPBuilderAdapter iocBuilderAdapter)
    {
        IoCBuilderAdapter = iocBuilderAdapter;
        InterfaceContext = new InterfaceContext();
        RegistryBuilder = new CommandRegistryBuilder(iocBuilderAdapter, InterfaceContext);
    }

    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> UseDefaultCommands()
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

    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> Configure(Action<InterfaceContext, TDPBuilder, CommandRegistryBuilder> configureAction)
    {
        try
        {
            configureAction(InterfaceContext, IoCBuilderAdapter.GetBuilder(), RegistryBuilder);
        }
        catch (Exception ex)
        {
            throw new ECFInitializationException(ex);
        }

        return this;
    }

    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> AddConfiguration(string configurationFileName = "appsettings.json", bool optional = false, bool reloadOnChange = true)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile(configurationFileName, optional, reloadOnChange)
            .Build();

        IoCBuilderAdapter.RegisterSingleton<IConfiguration>(configuration);

        return this;
    }

    public void Run(string[] args)
    {
        if (InterfaceContext.CommandScope == null)
            InterfaceContext.CommandScope = new CommandScope(IoCBuilderAdapter.Build());

        if (InterfaceContext.CommandScope == null)
            throw new ArgumentException("There were no CommandScope initialized. Please use Configure(Action<InterfaceContext, ContainerBuilder>) method first, before starting program.");

        CommandLineInterface commandInterface = new(InterfaceContext);

        commandInterface.Start(args);
    }
}
