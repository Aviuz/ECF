using Autofac;
using ECF.InverseOfControl;

namespace ECF.Autofac.Adapters;

public class AutoFacScopeAdapter : AutoFacProviderAdapter, IIoCScopeAdapter
{
    public AutoFacScopeAdapter(ILifetimeScope scope) : base(scope) { }

    public void Dispose() => scope.Dispose();
}
