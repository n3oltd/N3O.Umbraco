using System.Collections.Generic;

namespace N3O.Umbraco.StructuredData {
    public class JsonLd : Dictionary<string, object> {
        private JsonLd() { }

        public JsonLd OfType(string type) {
            Custom("@type", type);

            return this;
        }

        public JsonLd Custom(string key, object value) {
            this[key] = value;

            return this;
        }

        public JsonLd Nest(string key) {
            var jsonLd = New();

            this[key] = jsonLd;

            return jsonLd;
        }
        
        public static JsonLd Root() {
            var structuredData = New();
        
            structuredData.Custom("@context", "https://schema.org");

            return structuredData;
        }
    
        public static JsonLd New() {
            return new JsonLd();
        }
    }
}