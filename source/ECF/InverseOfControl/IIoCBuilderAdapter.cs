namespace ECF.InverseOfControl;

/// <summary>
/// ECF interface for interacting with IoC builders
/// </summary>
public interface IIoCBuilderAdapter
{
    public void RegisterSingleton<TService>(TService service) where TService : class;
    public void RegisterScoped<TService>() where TService : class;
    public void RegisterScoped<TInterface, TService>() where TInterface : class where TService : class, TInterface;

    /// <summary>
    /// This should configure target IoC to provide IIoCScopeAdapter inside components
    /// </summary>
    public void RegisterIoCProviderAdapter();
    public void RegisterTransient(Type serviceType);
    public IIoCProviderAdapter Build();
}

/// <inheritdoc cref="IIoCBuilderAdapter"/>
public interface IIoCBuilderAdapter<TBuilder> : IIoCBuilderAdapter
{
    public TBuilder GetBuilder();
}