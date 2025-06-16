using Microsoft.AspNetCore.Html;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Blocks;

public interface IBlocksRenderer {
    Task<HtmlString> RenderBlocksAsync(IPublishedContent content, string propertyName);
}