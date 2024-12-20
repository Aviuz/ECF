using ECF.InverseOfControl;
using Microsoft.Extensions.Hosting;

namespace ECF.HostedServices;

internal class HostedServicesRunner
{
    private readonly List<IHostedService> startedServices = new();
    private bool hasStarted = false;

    public async Task StartAsync(IIoCProviderAdapter iocProvider, CancellationToken cancellationToken)
    {
        if (hasStarted)
            return;

        hasStarted = true;

        foreach (var service in iocProvider.ResolveMultiple<IHostedService>())
        {
            await service.StartAsync(cancellationToken);
            startedServices.Add(service);
        }
    }

    public async Task StopAsync(IIoCProviderAdapter _)
    {
        foreach (var service in startedServices)
        {
            await service.StopAsync(default);
        }
    }
}
