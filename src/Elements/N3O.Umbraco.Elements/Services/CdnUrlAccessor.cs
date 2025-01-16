using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace N3O.Umbraco.Elements;

public class CdnUrlAccessor {
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CdnUrlAccessor(IWebHostEnvironment webHostEnvironment) {
        _webHostEnvironment = webHostEnvironment;
    }

    public string Get() {
        if (_webHostEnvironment.IsProduction()) {
            return "https://static.n3o.cloud";
        } else {
            return "https://static-beta.n3o.cloud";
        }
    }
}