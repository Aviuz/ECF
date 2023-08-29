using ECF.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace ECF;

public class ECFHostBuilder : ECFHostBuilderBase<MicrosoftServiceCollectionAdapter, IServiceCollection>
{
    public ECFHostBuilder() : base(new MicrosoftServiceCollectionAdapter(new ServiceCollection())) { }
}
