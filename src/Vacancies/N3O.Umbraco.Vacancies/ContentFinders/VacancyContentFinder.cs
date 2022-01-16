using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Vacancies.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Vacancies.ContentFinders {
    public class VacancyContentFinder : ContentFinderBase {
        public VacancyContentFinder(IContentCache contentCache) : base(contentCache) { }
        
        public override bool TryFindContentImpl(IPublishedRequestBuilder request) {
            return TryFindRelocatedContent(AliasHelper<VacanciesPage>.ContentTypeAlias(),
                                           AliasHelper<Vacancy>.ContentTypeAlias(),
                                           AliasHelper<Content.Vacancies>.ContentTypeAlias(),
                                           request);
        }
    }
}