using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IContentRenderabilityFilter {
    bool IsFilterFor(IPublishedContent content);
    Task<bool> CanRenderAsync(IPublishedContent content);
}
