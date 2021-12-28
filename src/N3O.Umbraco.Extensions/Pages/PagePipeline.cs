using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Pages {
    public class PagePipeline : IPagePipeline {
        private readonly IReadOnlyList<IPageExtension> _pageExtensions;

        public PagePipeline(IEnumerable<IPageExtension> pageExtensions) {
            _pageExtensions = pageExtensions.ToList();
        }
    
        public async Task<PageExtensionData> RunAsync(IPublishedContent page,
                                                      CancellationToken cancellationToken = default) {
            var extensionData = new PageExtensionData();
        
            foreach (var pageExtension in _pageExtensions) {
                var data = await pageExtension.ExecuteAsync(page, cancellationToken);

                if (data != null) {
                    extensionData.Add(pageExtension.Key, data);
                }
            }

            return extensionData;
        }
    }
}
