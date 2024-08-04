using ECF.Adapters;
using ECF.Engine;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddECFCommandRegistry(this IServiceCollection services, InterfaceContext interfaceContext, Action<CommandRegistryBuilder> configure)
    {
        services.TryAddSingleton(interfaceContext);

        var builder = new CommandRegistryBuilder(new MicrosoftServiceCollectionAdapter(services));
        configure(builder);

        return services;
    }

    public static CommandProcessor BuildAndCreateECFCommandProcessor(this IServiceCollection services)
    {
        return new CommandProcessor(new MicrosoftServiceProviderAdapter(services.BuildServiceProvider()));
    }
}