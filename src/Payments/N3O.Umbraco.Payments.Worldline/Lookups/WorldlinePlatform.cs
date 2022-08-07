using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Payments.Worldline.Lookups;

public abstract class WorldlinePlatform : NamedLookup {
    protected WorldlinePlatform(string id, string name) : base(id, name) { }

    public Uri GetEndpoint(IWebHostEnvironment webHostEnvironment) {
        string endpoint;
        
        if (webHostEnvironment.IsProduction()) {
            endpoint = GetProductionEndpoint();
        } else {
            endpoint = GetSandboxEndpoint();
        }

        return new Uri(endpoint);
    }

    protected abstract string GetSandboxEndpoint();
    protected abstract string GetProductionEndpoint();
}

public class OgoneWorldlinePlatform : WorldlinePlatform {
    public OgoneWorldlinePlatform() : base("ogone", "Ogone") { }
    
    protected override string GetSandboxEndpoint() => "https://eu.sandbox.api-ingenico.com";
    protected override string GetProductionEndpoint() => "https://eu.api-ingenico.com";
}

public class WorldlinePlatforms : StaticLookupsCollection<WorldlinePlatform> {
    public static readonly WorldlinePlatform Ogone = new OgoneWorldlinePlatform();
}
