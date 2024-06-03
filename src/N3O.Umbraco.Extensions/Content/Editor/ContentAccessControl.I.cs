using System.Threading.Tasks;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IContentAccessControl {
    Task<bool> CanEditAsync(IContent content);
    Task<bool> CanEditAsync(IPublishedContent content);
}