using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public class PagePipeline : IPagePipeline {
        private readonly IReadOnlyList<IPageModule> _pageModules;

        public PagePipeline(IEnumerable<IPageModule> pageModules) {
            _pageModules = pageModules.ToList();
        }
    
        public async Task<PageModuleData> RunAsync(IPublishedContent page,
                                                   CancellationToken cancellationToken = default) {
            var pageModuleData = new PageModuleData();
        
            foreach (var pageModule in _pageModules) {
                var data = await pageModule.ExecuteAsync(page, cancellationToken);

                if (data != null) {
                    pageModuleData.Add(pageModule.Key, data);
                }
            }

            return pageModuleData;
        }
    }
}
