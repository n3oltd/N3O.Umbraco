using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;

namespace N3O.Umbraco.Scheduler;

public class JobUrlProvider : IJobUrlProvider {
    private readonly IServer _server;
    
    public JobUrlProvider(IServer server) {
        _server = server;
    }
    
    public string GetBaseUrl() {
        var addressFeature = _server.Features.Get<IServerAddressesFeature>();
        var address = addressFeature.Addresses.
                                     First()
                                     .Replace("[::]", "localhost")
                                     .Replace("*", "localhost");

        return address;
    }
}