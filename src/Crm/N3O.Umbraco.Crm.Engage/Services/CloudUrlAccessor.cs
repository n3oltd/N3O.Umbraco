using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace N3O.Umbraco.Crm.Engage;

public class CloudUrlAccessor {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CloudUrlAccessor(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Get() {
        if (_webHostEnvironment.IsProduction()) {
            return "https://n3o.cloud";
        } else {
            return "https://beta.n3o.cloud";
        }
    }
}