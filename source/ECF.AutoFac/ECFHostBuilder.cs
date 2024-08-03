using Autofac;
using ECF.Autofac.Adapters;
using ECF.Engine;

namespace ECF.Autofac;

public class ECFHostBuilder : ECFHostBuilderBase<AutofacContainerBuilderAdapter, ContainerBuilder>
{
    public ECFHostBuilder() : base(new AutofacContainerBuilderAdapter(new ContainerBuilder())) { }
}
