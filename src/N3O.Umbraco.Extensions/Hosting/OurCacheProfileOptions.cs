using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using N3O.Umbraco.Constants;

namespace N3O.Umbraco.Hosting;

public class OurCacheProfileOptions : IConfigureOptions<MvcOptions> {
    public void Configure(MvcOptions options) {
        var noCacheProfile = new CacheProfile();
        noCacheProfile.NoStore = true;
        noCacheProfile.Location = ResponseCacheLocation.None;

        options.CacheProfiles.Add(CacheProfiles.NoCache, noCacheProfile);
    }
}
