using Autofac;
using ECF;
using ECF.AutoFac.Adapters;
using ECF.Engine;

namespace Autofac;

public static class DependencyInjectionExtensions
{
    public static CommandRegistryBuilder AddECFCommandRegistry(this ContainerBuilder containerBuilder)
    {
        return new CommandRegistryBuilder(new AutoFacContainerBuilderAdapter(containerBuilder));
    }

    public static CommandProcessor BuildAndCreateECFCommandProcessor(this ContainerBuilder containerBuilder)
    {
        return new CommandProcessor(new AutoFacProviderAdapter(containerBuilder.Build()));
    }
}
