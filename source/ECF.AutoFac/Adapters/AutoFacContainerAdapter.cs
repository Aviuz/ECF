using Autofac;
using ECF.InverseOfControl;

namespace ECF.AutoFac.Adapters;

public class AutoFacContainerAdapter : IIoCProviderAdapter
{
    private readonly IContainer container;

    public AutoFacContainerAdapter(IContainer container) => this.container = container;
    public IIoCScopeAdapter GetScope() => new AutoFacScopeAdapter(container.BeginLifetimeScope());
}
