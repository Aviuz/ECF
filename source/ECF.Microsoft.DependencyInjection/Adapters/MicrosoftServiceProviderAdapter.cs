using ECF.InverseOfControl;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ECF.Adapters;

public class MicrosoftServiceProviderAdapter : IIoCProviderAdapter
{
    private readonly IServiceProvider serviceProvider;

    public MicrosoftServiceProviderAdapter(IServiceProvider serviceProvider) => this.serviceProvider = serviceProvider;

    public TService Resolve<TService>() where TService : notnull => serviceProvider.GetRequiredService<TService>();
    public IEnumerable<TService> ResolveMultiple<TService>() where TService : notnull => serviceProvider.GetServices<TService>();
    public object Resolve(Type serviceType) => serviceProvider.GetRequiredService(serviceType);
    public IIoCScopeAdapter BeginNestedScope() => new MicrosoftServiceScopeAdapter(serviceProvider.CreateScope());
}
