using N3O.Umbraco.Content;
using N3O.Umbraco.Vacancies.Content;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Vacancies.UrlProviders {
    public class VacanciesUrlProvider : UrlProviderBase {
        private static readonly string VacanciesPageAlias = AliasHelper<VacanciesPageContent>.ContentTypeAlias();
        private static readonly string VacancyAlias = AliasHelper<VacancyContent>.ContentTypeAlias();
        
        public VacanciesUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            return TryGetRelocatedUrl(VacanciesPageAlias, VacancyAlias, content, mode, culture, current);
        }
    }
}