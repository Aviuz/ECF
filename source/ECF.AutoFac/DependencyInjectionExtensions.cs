using ECF;
using ECF.Autofac.Adapters;
using ECF.Engine;

namespace Autofac;

public static class DependencyInjectionExtensions
{
    public static CommandRegistryBuilder AddECFCommandRegistry(this ContainerBuilder containerBuilder)
    {
        return new CommandRegistryBuilder(new AutofacContainerBuilderAdapter(containerBuilder));
    }

    public static CommandProcessor BuildAndCreateECFCommandProcessor(this ContainerBuilder containerBuilder)
    {
        return new CommandProcessor(new AutoFacProviderAdapter(containerBuilder.Build()));
    }
}
