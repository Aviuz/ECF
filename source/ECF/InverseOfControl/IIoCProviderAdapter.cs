namespace ECF.InverseOfControl;

/// <summary>
/// ECF interface for interacting with IoC providers
/// </summary>
public interface IIoCProviderAdapter
{
    public IIoCScopeAdapter BeginNestedScope();
    public TService Resolve<TService>() where TService : notnull;
    public IEnumerable<TService> ResolveMultiple<TService>() where TService : notnull;
    public object Resolve(Type serviceType);
}