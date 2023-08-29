using Autofac;
using ECF.Autofac.Adapters;

namespace ECF.Autofac;

public class ECFHostBuilder : ECFHostBuilderBase<AutofacContainerBuilderAdapter, ContainerBuilder>
{
    public ECFHostBuilder() : base(new AutofacContainerBuilderAdapter(new ContainerBuilder())) { }
}
