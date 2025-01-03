using Autofac;
using ECF.InverseOfControl;

namespace ECF.Autofac.Adapters;

public class AutoFacProviderAdapter : IIoCProviderAdapter
{
    protected readonly ILifetimeScope scope;

    public AutoFacProviderAdapter(ILifetimeScope scope) => this.scope = scope;
    public TService Resolve<TService>() where TService : notnull => scope.Resolve<TService>();
    public IEnumerable<TService> ResolveMultiple<TService>() where TService : notnull => scope.Resolve<IEnumerable<TService>>();
    public object Resolve(Type serviceType) => scope.Resolve(serviceType);
    public IIoCScopeAdapter BeginNestedScope() => new AutoFacScopeAdapter(scope.BeginLifetimeScope());
}
