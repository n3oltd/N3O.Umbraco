using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using System.Buffers;

namespace N3O.Umbraco.Hosting {
    public class OurJsonOutputFormatter : NewtonsoftJsonOutputFormatter {
        public OurJsonOutputFormatter(JsonSerializerSettings serializerSettings,
                                      ArrayPool<char> charPool,
                                      MvcOptions mvcOptions)
            : base(serializerSettings, charPool, mvcOptions) { }

        protected override JsonSerializer CreateJsonSerializer(OutputFormatterWriteContext context) {
            var jsonProvider = (IJsonProvider) context.HttpContext.RequestServices.GetService(typeof(IJsonProvider));
            var jsonSettings = jsonProvider.GetSettings();
            var jsonSerializer = JsonSerializer.Create(jsonSettings);

            return jsonSerializer;
        }
    }
}
