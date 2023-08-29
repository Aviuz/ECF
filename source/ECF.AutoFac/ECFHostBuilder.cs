using Autofac;
using ECF.AutoFac.Adapters;

namespace ECF.AutoFac;

public class ECFHostBuilder : ECFHostBuilderBase<AutoFacContainerBuilderAdapter, ContainerBuilder>
{
    public ECFHostBuilder() : base(new AutoFacContainerBuilderAdapter(new ContainerBuilder())) { }
}
