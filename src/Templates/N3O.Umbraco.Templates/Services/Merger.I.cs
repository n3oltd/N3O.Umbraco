using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Templates;

public interface IMerger {
    Task<string> MergeForAsync(IPublishedContent content, string markup, CancellationToken cancellationToken = default);

    Task<IHtmlContent> MergePartialForAsync(IPublishedContent content,
                                            ViewContext viewContext,
                                            string partialViewName,
                                            object model,
                                            CancellationToken cancellationToken = default);
    
    Task<IHtmlContent> MergePartialForCurrentContentAsync(ViewContext viewContext,
                                                          string partialViewName,
                                                          object model,
                                                          CancellationToken cancellationToken = default);
}