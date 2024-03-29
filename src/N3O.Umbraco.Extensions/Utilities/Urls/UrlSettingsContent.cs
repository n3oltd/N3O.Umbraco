using Flurl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;

namespace N3O.Umbraco.Utilities;

public class UrlSettingsContent : UmbracoContent<UrlSettingsContent> {
    public string DevelopmentBaseUrl => GetValue(x => x.DevelopmentBaseUrl);
    public string ProductionBaseUrl => GetValue(x => x.ProductionBaseUrl);
    public string StagingBaseUrl => GetValue(x => x.StagingBaseUrl);
    
    public Url BaseUrl(IWebHostEnvironment webHostEnvironment) {
        if (webHostEnvironment.IsProduction()) {
            return ProductionBaseUrl;
        } else if (webHostEnvironment.IsStaging()) {
            return StagingBaseUrl;
        } else if (webHostEnvironment.IsDevelopment()) {
            return DevelopmentBaseUrl;
        } else {
            throw UnrecognisedValueException.For(webHostEnvironment.EnvironmentName);
        }
    }
}
