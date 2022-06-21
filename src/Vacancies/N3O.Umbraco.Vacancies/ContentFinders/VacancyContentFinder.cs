using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Vacancies.Content;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Vacancies.ContentFinders {
    public class VacancyContentFinder : ContentFinder {
        private static readonly string VacanciesPageAlias = AliasHelper<VacanciesPageContent>.ContentTypeAlias();
        private static readonly string VacancyAlias = AliasHelper<VacancyContent>.ContentTypeAlias();
        private static readonly string VacanciesAlias = AliasHelper<VacanciesContent>.ContentTypeAlias();
        
        public VacancyContentFinder(ILogger<VacancyContentFinder> logger, IContentCache contentCache)
            : base(logger, contentCache) { }

        protected override Task<bool> FindContentAsync(IPublishedRequestBuilder request) {
            return TryFindRelocatedContentAsync(VacanciesPageAlias, VacancyAlias, VacanciesAlias, request);
        }
    }
}