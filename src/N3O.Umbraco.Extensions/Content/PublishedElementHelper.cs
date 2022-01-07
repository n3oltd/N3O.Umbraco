using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public class PublishedElementHelper : IPublishedElementHelper {
        private readonly IJsonProvider _jsonProvider;

        public PublishedElementHelper(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }
        
        public JObject ToJObject(IPublishedElement content) {
            var jObject = new JObject();
            
            jObject.Add(nameof(IPublishedElement.Key), new JValue(content.Key));
            jObject.Add(nameof(IPublishedElement.ContentType), new JValue(content.ContentType.Alias));

            foreach (var property in content.Properties) {
                var jToken = property.GetValue().ToJToken(_jsonProvider);
                
                jObject.Add(property.Alias, jToken);
            }

            return jObject;
        }
    }
}
