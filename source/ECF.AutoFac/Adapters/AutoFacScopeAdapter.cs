using Autofac;
using ECF.InverseOfControl;

namespace ECF.AutoFac.Adapters;

public class AutoFacScopeAdapter : AutoFacProviderAdapter, IIoCScopeAdapter
{
    public AutoFacScopeAdapter(ILifetimeScope scope) : base(scope) { }
    
    public void Dispose() => scope.Dispose();
}
