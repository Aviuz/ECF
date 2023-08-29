using ECF.Microsoft.DependencyInjection.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace ECF.Microsoft.DependencyInjection;

public class ECFHostBuilder : ECFHostBuilderBase<MicrosoftServiceCollectionAdapter, IServiceCollection>
{
    public ECFHostBuilder() : base(new MicrosoftServiceCollectionAdapter(new ServiceCollection())) { }
}
