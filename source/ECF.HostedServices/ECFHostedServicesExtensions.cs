using ECF.Engine;
using ECF.HostedServices;
using ECF.InverseOfControl;
using Microsoft.Extensions.Hosting;

namespace ECF;

public static class ECFHostedServicesExtensions
{
    public static ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> AddHostedServices<TDPBuilderAdapter, TDPBuilder>(this ECFHostBuilderBase<TDPBuilderAdapter, TDPBuilder> builder) where TDPBuilderAdapter : class, IIoCBuilderAdapter<TDPBuilder> where TDPBuilder : class
    {
        HostedServicesRunner runner = new();

        builder.RegisterStartHandler(runner.StartAsync);
        builder.RegisterStopHandler(runner.StopAsync);
        return builder;
    }
}