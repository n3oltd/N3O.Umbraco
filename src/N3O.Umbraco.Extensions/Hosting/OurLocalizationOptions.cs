using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace N3O.Umbraco.Hosting;

public class OurRequestLocalizationOptions : IConfigureOptions<RequestLocalizationOptions> {
    public void Configure(RequestLocalizationOptions options) {
        options.RequestCultureProviders.Add(new OurRequestCultureProvider());
    }
}