using Autofac;
using ECF.InverseOfControl;

namespace ECF.AutoFac.Adapters;

public class AutoFacScopeAdapter : IIoCScopeAdapter
{
    private readonly ILifetimeScope scope;

    public AutoFacScopeAdapter(ILifetimeScope scope) => this.scope = scope;
    public TService Resolve<TService>() where TService : notnull => scope.Resolve<TService>();
    public object Resolve(Type serviceType) => scope.Resolve(serviceType);
    public IIoCScopeAdapter BeginNestedScope() => new AutoFacScopeAdapter(scope.BeginLifetimeScope());
    public void Dispose() => scope.Dispose();
}
