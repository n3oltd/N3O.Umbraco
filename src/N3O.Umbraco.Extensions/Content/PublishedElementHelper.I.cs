using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public interface IPublishedElementHelper {
        JObject ToJObject(IPublishedElement content);
    }
}
