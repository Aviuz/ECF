using ECF;
using ECF.Engine;
using ECF.Adapters;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddECFCommandRegistry(this IServiceCollection services, Action<CommandRegistryBuilder> configure)
    {
        var builder = new CommandRegistryBuilder(new MicrosoftServiceCollectionAdapter(services));
        configure(builder);
        return services;
    }

    public static CommandProcessor BuildAndCreateECFCommandProcessor(this IServiceCollection services)
    {
        return new CommandProcessor(new MicrosoftServiceProviderAdapter(services.BuildServiceProvider()));
    }
}