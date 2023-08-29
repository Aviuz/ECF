using Autofac;
using ECF.InverseOfControl;

namespace ECF.AutoFac.Adapters;

public class AutoFacContainerBuilderAdapter : IIoCBuilderAdapter<ContainerBuilder>
{
    private readonly ContainerBuilder containerBuilder;

    public AutoFacContainerBuilderAdapter(ContainerBuilder containerBuilder)
    {
        this.containerBuilder = containerBuilder;
    }

    public ContainerBuilder GetBuilder() => containerBuilder;

    public void RegisterSingleton<TService>(TService service) where TService : class
    {
        containerBuilder.RegisterInstance(service);
    }

    public void RegisterTransient(Type serviceType)
    {
        containerBuilder.RegisterType(serviceType).InstancePerDependency();
    }

    public IIoCProviderAdapter Build()
    {
        return new AutoFacProviderAdapter(containerBuilder.Build());
    }

    public void RegisterScoped<TService>() where TService : class
    {
        containerBuilder.RegisterType<TService>().InstancePerLifetimeScope();
    }

    public void RegisterScoped<TInterface, TService>() where TInterface : class where TService : class, TInterface
    {
        containerBuilder.RegisterType<TService>().As<TInterface>().InstancePerLifetimeScope();
    }

    public void RegisterIoCProviderAdapter()
    {
        containerBuilder.RegisterType<AutoFacProviderAdapter>().As<IIoCProviderAdapter>().InstancePerDependency();
    }
}
