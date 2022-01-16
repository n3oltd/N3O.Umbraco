using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Vacancies.Content;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Vacancies.ContentFinders {
    public class VacancyContentFinder : ContentFinderBase {
        private static readonly string VacanciesPageAlias = AliasHelper<VacanciesPageContent>.ContentTypeAlias();
        private static readonly string VacancyAlias = AliasHelper<VacancyContent>.ContentTypeAlias();
        private static readonly string VacanciesAlias = AliasHelper<VacanciesContent>.ContentTypeAlias();
        
        public VacancyContentFinder(IContentCache contentCache) : base(contentCache) { }
        
        public override bool TryFindContentImpl(IPublishedRequestBuilder request) {
            return TryFindRelocatedContent(VacanciesPageAlias, VacancyAlias, VacanciesAlias, request);
        }
    }
}