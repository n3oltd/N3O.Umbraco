using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public class NotRenderableFilter : IContentRenderabilityFilter {
    public bool IsFilterFor(IPublishedContent content) => true;

    public Task<bool> CanRenderAsync(IPublishedContent content) {
        return Task.FromResult(!content.GetType().ImplementsInterface<INotRenderable>());
    }
}
