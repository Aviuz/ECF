using ECF;
using ECF.Engine;
using ECF.Adapters;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjectionExtensions
{
    public static CommandRegistryBuilder AddECFCommandRegistry(this IServiceCollection services)
    {
        return new CommandRegistryBuilder(new MicrosoftServiceCollectionAdapter(services));
    }

    public static CommandProcessor BuildAndCreateECFCommandProcessor(this IServiceCollection services)
    {
        return new CommandProcessor(new MicrosoftServiceProviderAdapter(services.BuildServiceProvider()));
    }
}