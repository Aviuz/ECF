namespace ECF.InverseOfControl;

public interface IIoCProviderAdapter
{
    public IIoCScopeAdapter GetScope();
}

public interface IIoCProviderAdapter<TProvider> : IIoCProviderAdapter
{
    public TProvider GetProvider();
}