namespace ECF.InverseOfControl;

public interface IIoCScopeAdapter : IDisposable
{
    public IIoCScopeAdapter BeginNestedScope();
    public TService Resolve<TService>() where TService : notnull;
    public object Resolve(Type serviceType);
}