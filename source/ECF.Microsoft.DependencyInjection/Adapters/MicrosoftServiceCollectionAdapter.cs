using ECF.InverseOfControl;
using Microsoft.Extensions.DependencyInjection;

namespace ECF.Microsoft.DependencyInjection.Adapters;

public class MicrosoftServiceCollectionAdapter : IIoCBuilderAdapter<IServiceCollection>
{
    private readonly IServiceCollection services;

    public MicrosoftServiceCollectionAdapter(IServiceCollection services)
    {
        this.services = services;
    }

    public IServiceCollection GetBuilder() => services;

    public void RegisterSingleton<TService>(TService service) where TService : class
    {
        services.AddSingleton(service);
    }

    public void RegisterTransient(Type serviceType)
    {
        services.AddTransient(serviceType);
    }

    public IIoCProviderAdapter Build()
    {
        return new MicrosoftServiceProviderAdapter(services.BuildServiceProvider());
    }

    public void RegisterScoped<TService>() where TService : class
    {
        services.AddScoped<TService>();
    }

    public void RegisterScoped<TInterface, TService>() where TInterface : class where TService : class, TInterface
    {
        services.AddScoped<TInterface, TService>();
    }

    public void RegisterIoCProviderAdapter()
    {
        services.AddTransient<IIoCProviderAdapter, MicrosoftServiceProviderAdapter>();
    }
}
