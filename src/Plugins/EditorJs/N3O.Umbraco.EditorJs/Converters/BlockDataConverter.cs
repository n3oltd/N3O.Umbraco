using N3O.Umbraco.EditorJs.Models;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace N3O.Umbraco.EditorJs;

public abstract class BlockDataConverter<T> : IBlockDataConverter where T : class {
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IPublishedUrlProvider _publishedUrlProvider;

    protected BlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                 IPublishedUrlProvider publishedUrlProvider) {
        _umbracoContextAccessor = umbracoContextAccessor;
        _publishedUrlProvider = publishedUrlProvider;
    }
    
    public bool CanConvert(string typeId) {
        return TypeId.EqualsInvariant(typeId);
    }
    
    public EditorJsBlock Convert(string id, string typeId, JsonSerializer serializer, JObject data) {
        var typedData = data.ToObject<T>(serializer);

        Process(typedData);

        return new EditorJsBlock<T>(id, typeId, typedData);
    }
    
    protected abstract string TypeId { get; }

    protected virtual void Process(T data) { }

    protected string ConvertUmbracoLinks(string text) {
        return Regex.Replace(text, "(<a\\s+(?:[^>]*?\\s+)?href=\")(umb:\\/\\/[^\"]*)\"", ConvertUdiUrl);
    }

    private string ConvertUdiUrl(Match match) {
        var udiText = match.Groups[2].Value;
        var udi = UdiParser.Parse(udiText);

        if (_umbracoContextAccessor.TryGetUmbracoContext(out var context)) {
            var content = context.Content?.GetById(udi);
            
            if (content != null) {
                return $"{match.Groups[1].Value}{content.Url(_publishedUrlProvider)}\"";
            }
        }
        
        return "";
    }
}