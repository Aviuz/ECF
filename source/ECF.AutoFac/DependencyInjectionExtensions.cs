using ECF.Autofac.Adapters;
using ECF.Engine;

namespace Autofac;

public static class DependencyInjectionExtensions
{
    public static ContainerBuilder AddECFCommandRegistry(this ContainerBuilder containerBuilder, InterfaceContext interfaceContext, Action<CommandRegistryBuilder> configure)
    {
        containerBuilder.RegisterInstance(interfaceContext);

        var builder = new CommandRegistryBuilder(new AutofacContainerBuilderAdapter(containerBuilder));
        configure(builder);

        return containerBuilder;
    }

    public static CommandProcessor BuildAndCreateECFCommandProcessor(this ContainerBuilder containerBuilder)
    {
        return new CommandProcessor(new AutoFacProviderAdapter(containerBuilder.Build()));
    }
}
