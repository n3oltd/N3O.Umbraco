using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public class PagePipeline : IPagePipeline {
        private readonly IReadOnlyList<IPageModule> _modules;

        public PagePipeline(IEnumerable<IPageModule> modules) {
            _modules = modules.ToList();
        }
    
        public async Task<PageModulesData> RunAsync(IPublishedContent page,
                                                    CancellationToken cancellationToken = default) {
            var modulesData = new PageModulesData();
        
            foreach (var module in _modules.Where(x => x.ShouldExecute(page))) {
                var data = await module.ExecuteAsync(page, cancellationToken);

                if (data != null) {
                    modulesData.Add(module.Key, data);
                }
            }

            return modulesData;
        }
    }
}