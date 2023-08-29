namespace ECF.InverseOfControl;

public interface IIoCBuilderAdapter
{
    public void RegisterSingleton<TService>(TService service) where TService : class;
    public void RegisterScoped<TService>() where TService : notnull;
    public void RegisterScoped<TInterface, TService>() where TInterface : notnull where TService : notnull;

    /// <summary>
    /// This should configure target IoC to provide IIoCScopeAdapter inside components
    /// </summary>
    public void RegisterIoCScopeAdapter();
    public void RegisterTransient(Type serviceType);
    public IIoCProviderAdapter Build();
}

public interface IIoCBuilderAdapter<TBuilder> : IIoCBuilderAdapter
{
    public TBuilder GetBuilder();
}