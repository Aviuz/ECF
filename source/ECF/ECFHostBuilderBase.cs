using ECF.Engine;
using ECF.Exceptions;
using ECF.InverseOfControl;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ECF;

public class ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> where TDPBuilderAdapter : class, IIoCBuilderAdapter<TDPBuilder> where TDPBuilder : class
{
    public InterfaceContext InterfaceContext { get; }
    public TDPBuilderAdapter IoCBuilderAdapter { get; }
    public CommandRegistryBuilder RegistryBuilder { get; }

    public ECFHostBuilderBase(TDPBuilderAdapter iocBuilderAdapter)
    {
        InterfaceContext = new InterfaceContext();
        IoCBuilderAdapter = iocBuilderAdapter;
        RegistryBuilder = new CommandRegistryBuilder(iocBuilderAdapter);

        iocBuilderAdapter.RegisterSingleton(InterfaceContext);
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
        if (InterfaceContext.CommandProcessor == null)
            InterfaceContext.CommandProcessor = new CommandProcessor(IoCBuilderAdapter.Build());

        CommandLineInterface commandInterface = new(InterfaceContext);

        commandInterface.Start(args);
    }
}
