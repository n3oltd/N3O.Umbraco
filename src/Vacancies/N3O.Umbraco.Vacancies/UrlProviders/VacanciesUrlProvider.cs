using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Vacancies.Content;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using UrlProvider = N3O.Umbraco.UrlProviders.UrlProvider;

namespace N3O.Umbraco.Vacancies.UrlProviders {
    public class VacanciesUrlProvider : UrlProvider {
        private static readonly string VacanciesPageAlias = AliasHelper<VacanciesPageContent>.ContentTypeAlias();
        private static readonly string VacancyAlias = AliasHelper<VacancyContent>.ContentTypeAlias();
        
        public VacanciesUrlProvider(ILogger<VacanciesUrlProvider> logger,
                                    DefaultUrlProvider defaultUrlProvider,
                                    IContentCache contentCache)
            : base(logger, defaultUrlProvider, contentCache) { }

        protected override UrlInfo ResolveUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            return TryGetRelocatedUrl(VacanciesPageAlias, VacancyAlias, content, mode, culture, current);
        }
    }
}