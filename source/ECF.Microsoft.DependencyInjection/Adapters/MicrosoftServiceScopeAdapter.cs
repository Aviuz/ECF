using ECF.InverseOfControl;
using Microsoft.Extensions.DependencyInjection;

namespace ECF.Adapters;

public class MicrosoftServiceScopeAdapter : MicrosoftServiceProviderAdapter, IIoCScopeAdapter
{
    private readonly IServiceScope serviceScope;

    public MicrosoftServiceScopeAdapter(IServiceScope serviceScope) : base(serviceScope.ServiceProvider)
        => this.serviceScope = serviceScope;

    public void Dispose() => serviceScope.Dispose();
}
