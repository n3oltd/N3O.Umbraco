using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Analytics.Content;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Analytics.Modules;

public class GoogleAnalytics4PageModule : IPageModule {
    private readonly IContentCache _contentCache;

    public GoogleAnalytics4PageModule(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public bool ShouldExecute(IPublishedContent page) {
        var analyticsSettings = _contentCache.Single<GoogleAnalytics4SettingsContent>();

        return analyticsSettings.HasValue(x => x.MeasurementId);
    }

    public Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var analyticsSettings = _contentCache.Single<GoogleAnalytics4SettingsContent>();

        var code = new HtmlString(@"
<!-- Global site tag (gtag.js) - Google Analytics -->
<script async src=""https://www.googletagmanager.com/gtag/js?id=" + analyticsSettings.MeasurementId + @"""></script>
<script>
window.dataLayer = window.dataLayer || [];
function gtag() { dataLayer.push(arguments); }
gtag('js', new Date());

gtag('config', '" + analyticsSettings.MeasurementId + @"');
</script>");

        return Task.FromResult<object>(new Code(code));
    }

    public string Key => AnalyticsConstants.PageModuleKeys.GoogleAnalytics4;
}
