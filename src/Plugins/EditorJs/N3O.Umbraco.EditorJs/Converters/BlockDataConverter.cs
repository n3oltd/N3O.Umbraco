using N3O.Umbraco.EditorJs.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using System.Web;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace N3O.Umbraco.EditorJs;

public abstract class BlockDataConverter<TData> : IBlockDataConverter where TData : class {
    private const string DocumentEntityType = global::Umbraco.Cms.Core.Constants.UdiEntityType.Document;
    private const string MediaEntityType = global::Umbraco.Cms.Core.Constants.UdiEntityType.Media;
    
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
    
    public EditorJsBlock Convert(string id, string typeId, JsonSerializer serializer, JObject data, JObject tunesData) {
        var typedData = data.ToObject<TData>(serializer);

        Process(typedData);

        return new EditorJsBlock<TData>(id, typeId, typedData, tunesData);
    }
    
    protected abstract string TypeId { get; }

    protected virtual void Process(TData data) { }

    protected string ConvertUmbracoLinks(string text) {
        return Regex.Replace(text, "(<a\\s+(?:[^>]*?\\s+)?href=\")(umb:\\/\\/[^\"]*)\"", ConvertUdiUrl);
    }

    protected string DecodePlatformsElements(string text) {
        var encodedStart = HttpUtility.HtmlEncode(EditorJsConstants.Delimiters.PlatformsElements.Start);
        var encodedEnd = HttpUtility.HtmlEncode(EditorJsConstants.Delimiters.PlatformsElements.End);

        return Regex.Replace(text,
                             encodedStart + "([.+])" + encodedEnd,
                             m => HttpUtility.HtmlDecode(m.Groups[1].Value));
    }

    private string ConvertUdiUrl(Match match) {
        var udiText = match.Groups[2].Value;
        var udi = UdiParser.Parse(udiText);

        if (_umbracoContextAccessor.TryGetUmbracoContext(out var context)) {
            IPublishedContent content;
            
            if (udi.EntityType == DocumentEntityType) {
                content = context.Content?.GetById(udi);
            } else if (udi.EntityType == MediaEntityType) {
                content = context.Media?.GetById(udi);
            } else {
                throw UnrecognisedValueException.For(udi.EntityType);
            }
            
            if (content != null) {
                return $"{match.Groups[1].Value}{content.Url(_publishedUrlProvider)}\"";
            }
        }
        
        return "";
    }
}