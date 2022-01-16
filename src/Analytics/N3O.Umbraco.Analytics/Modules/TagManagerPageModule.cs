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

namespace N3O.Umbraco.Analytics.Modules {
    public class TagManagerPageModule : IPageModule {
        private readonly Lazy<IContentCache> _contentCache;

        public TagManagerPageModule(Lazy<IContentCache> contentCache) {
            _contentCache = contentCache;
        }

        public bool ShouldExecute(IPublishedContent page) => true;

            public Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
            var body = GetCode(x => x.Body);
            var head = GetCode(x => x.Head);

            return Task.FromResult<object>(new TagManagerCode(body, head));
        }

        public string Key => AnalyticsConstants.PageModuleKeys.TagManager;

        private HtmlString GetCode(Func<TagManagerSettingsContent, string> getCode) {
            var tagManagerSettings = _contentCache.Value.Single<TagManagerSettingsContent>();

            return tagManagerSettings.IfNotNull(x => getCode(x)?.ToHtmlString());
        }
    }
}
