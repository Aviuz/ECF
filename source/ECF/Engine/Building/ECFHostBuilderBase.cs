using ECF.Exceptions;
using ECF.InverseOfControl;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ECF.Engine;

/// <summary>
/// This is base class for actual builders in IoC dedicated frameworks. It consists of operations required to configure ECF engine.
/// </summary>
public class ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> where TDPBuilderAdapter : class, IIoCBuilderAdapter<TDPBuilder> where TDPBuilder : class
{
    private bool throwWhenCommandRegistryNotEmpty = false;

    public delegate Task OnStartEventHandler(IIoCProviderAdapter services, CancellationToken cancellationToken);
    public delegate Task OnStopEventHandler(IIoCProviderAdapter services);
    private event OnStartEventHandler onStart;
    private event OnStopEventHandler onStop;

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

            if (InterfaceContext.DefaultCommand == null)
                InterfaceContext.DefaultCommand = typeof(BaseKitCommands.HelpCommand);
        }
        catch (Exception ex)
        {
            throw new ECFInitializationException(ex);
        }

        return this;
    }

    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> UseSingleCommand<TCommand>() where TCommand : class, ICommand
    {
        try
        {
            IoCBuilderAdapter.RegisterScoped<TCommand>();
            InterfaceContext.DefaultCommand = typeof(TCommand);
            InterfaceContext.DisablePrompting = true;

            throwWhenCommandRegistryNotEmpty = true;
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
            .AddUserSecrets(Assembly.GetEntryAssembly())
            .Build();

        IoCBuilderAdapter.RegisterSingleton<IConfiguration>(configuration);

        return this;
    }

    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> RegisterDefaultCommand<TCommandType>() => RegisterDefaultCommand(typeof(TCommandType));
    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> RegisterDefaultCommand(Type type)
    {
        IoCBuilderAdapter.RegisterTransient(type);
        InterfaceContext.DefaultCommand = type;
        return this;
    }

    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> RegisterStartHandler(OnStartEventHandler eventHandler)
    {
        onStart += eventHandler;
        return this;
    }

    public ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> RegisterStopHandler(OnStopEventHandler eventHandler)
    {
        onStop += eventHandler;
        return this;
    }

    public void Run(string[] args) => RunAsync(args, default).Wait();

    public async Task RunAsync(string[] args, CancellationToken cancellationToken = default)
    {
        using CancellationTokenSource customBackgroundWorkersCts = new();

        if (throwWhenCommandRegistryNotEmpty && RegistryBuilder.IsEmpty == false)
            throw new ECFCommandRegistryNotEmptyWhenUsingSingleMode();

        IIoCProviderAdapter ioc = IoCBuilderAdapter.Build();

        if (InterfaceContext.CommandProcessor == null)
            InterfaceContext.CommandProcessor = new CommandProcessor(ioc);

        if (onStart != null)
            foreach (OnStartEventHandler start in onStart.GetInvocationList())
                await start(ioc, customBackgroundWorkersCts.Token);

        CommandLineInterface commandInterface = new(InterfaceContext);
        await commandInterface.RunAsync(args, cancellationToken);

        if (onStop != null)
            foreach (OnStopEventHandler stop in onStop.GetInvocationList())
                await stop(ioc);

        customBackgroundWorkersCts.Cancel();
    }
}
