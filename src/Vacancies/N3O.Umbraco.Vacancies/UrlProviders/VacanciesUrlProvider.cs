using N3O.Umbraco.Content;
using N3O.Umbraco.Vacancies.Content;
using N3O.Umbraco.UrlProviders;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Vacancies.UrlProviders {
    public class VacanciesUrlProvider : UrlProviderBase {
        public VacanciesUrlProvider(DefaultUrlProvider defaultUrlProvider, IContentCache contentCache)
            : base(defaultUrlProvider, contentCache) { }
        
        public override UrlInfo GetUrl(IPublishedContent content, UrlMode mode, string culture, Uri current) {
            return TryGetRelocatedUrl(AliasHelper<VacanciesPage>.ContentTypeAlias(),
                                      AliasHelper<Vacancy>.ContentTypeAlias(),
                                      content,
                                      mode,
                                      culture,
                                      current);
        }
    }
}