using ECF.Adapters;
using ECF.Engine;
using Microsoft.Extensions.DependencyInjection;

namespace ECF;

public class ECFHostBuilder : ECFHostBuilderBase<MicrosoftServiceCollectionAdapter, IServiceCollection>
{
    public ECFHostBuilder() : base(new MicrosoftServiceCollectionAdapter(new ServiceCollection())) { }
}
