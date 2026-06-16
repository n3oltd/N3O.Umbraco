using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;

namespace N3O.Umbraco.Hosting;

public class OurRequestLocalizationOptions : IConfigureOptions<RequestLocalizationOptions> {
    public void Configure(RequestLocalizationOptions options) {
        var providers = options.RequestCultureProviders;
        
        var insertAt = IndexOfAcceptLanguageProvider(providers);

        providers.Insert(insertAt, new OurDomainRequestCultureProvider(options));
        providers.Insert(insertAt, new OurRequestCultureProvider());
    }

    private static int IndexOfAcceptLanguageProvider(IList<IRequestCultureProvider> providers) {
        for (var i = 0; i < providers.Count; i++) {
            if (providers[i] is AcceptLanguageHeaderRequestCultureProvider) {
                return i;
            }
        }

        return providers.Count;
    }
}