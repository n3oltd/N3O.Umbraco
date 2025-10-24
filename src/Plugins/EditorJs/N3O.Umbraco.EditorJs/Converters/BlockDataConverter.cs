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

public abstract class BlockDataConverter<TData, TTunes> : IBlockDataConverter where TData : class where TTunes : class {
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
    
    public EditorJsBlock Convert(string id, string typeId, JsonSerializer serializer, JObject data, JObject tunes) {
        var typedData = data.ToObject<TData>(serializer);
        var typedTunes = tunes?.ToObject<TTunes>(serializer);

        Process(typedData);

        return new EditorJsBlock<TData, TTunes>(id, typeId, typedData, typedTunes);
    }
    
    protected abstract string TypeId { get; }

    protected virtual void Process(TData data) { }

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