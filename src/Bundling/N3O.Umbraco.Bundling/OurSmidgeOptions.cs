using Microsoft.Extensions.Options;
using Smidge.Cache;
using Smidge.Options;
using System;
using Umbraco.Community.Smidge;

namespace N3O.Umbraco.Bundling;

// Mirrors Umbraco 13's SmidgeOptionsSetup, which Umbraco.Community.Smidge does not carry over, so without this the
// configured cache buster and in-memory caching never reach the bundles the Bundler generates. n3oltd/work#3211
public class OurSmidgeOptions : IConfigureOptions<SmidgeOptions> {
    private readonly IOptions<RuntimeMinificationSettings> _runtimeMinificationSettings;

    public OurSmidgeOptions(IOptions<RuntimeMinificationSettings> runtimeMinificationSettings) {
        _runtimeMinificationSettings = runtimeMinificationSettings;
    }

    public void Configure(SmidgeOptions options) {
        var settings = _runtimeMinificationSettings.Value;

        options.CacheOptions.UseInMemoryCache = settings.UseInMemoryCache ||
                                                settings.CacheBuster == RuntimeMinificationCacheBuster.Timestamp;

        var cacheBusterType = GetCacheBusterType(settings.CacheBuster);

        if (cacheBusterType != null) {
            options.DefaultBundleOptions.DebugOptions.SetCacheBusterType(cacheBusterType);
            options.DefaultBundleOptions.ProductionOptions.SetCacheBusterType(cacheBusterType);
        }
    }

    // Version maps to Umbraco.Community.Smidge's own cache buster, which is internal to that package and so cannot
    // be named here; leaving it unset falls back to Smidge's ConfigCacheBuster. No site configures Version
    private Type GetCacheBusterType(RuntimeMinificationCacheBuster cacheBuster) {
        if (cacheBuster == RuntimeMinificationCacheBuster.Timestamp) {
            return typeof(TimestampCacheBuster);
        } else if (cacheBuster == RuntimeMinificationCacheBuster.AppDomain) {
            return typeof(AppDomainLifetimeCacheBuster);
        } else {
            return null;
        }
    }
}
