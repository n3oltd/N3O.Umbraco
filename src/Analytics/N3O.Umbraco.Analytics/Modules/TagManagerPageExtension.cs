using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Analytics.Content;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Analytics.Modules;

public class TagManagerPageExtension : IPageExtension {
    private readonly IContentCache _contentCache;

    public TagManagerPageExtension(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        var body = GetCode(x => x.Body);
        var head = GetCode(x => x.Head);

        return Task.FromResult<object>(new TagManagerCode(body, head));
    }

    public string Key => AnalyticsConstants.Keys.TagManager;

    private HtmlString GetCode(Func<TagMangerSettings, string> getCode) {
        var tagManagerSettings = _contentCache.Single<TagMangerSettings>();

        return tagManagerSettings.IfNotNull(x => getCode(x)?.ToHtmlString());
    }
}
