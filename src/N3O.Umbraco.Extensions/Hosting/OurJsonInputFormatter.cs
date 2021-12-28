using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using N3O.Umbraco.Json;
using Newtonsoft.Json;
using System.Buffers;

namespace N3O.Umbraco.Hosting {
    public class OurJsonInputFormatter : NewtonsoftJsonInputFormatter {
        public OurJsonInputFormatter(ILogger logger,
                                     JsonSerializerSettings serializerSettings,
                                     ArrayPool<char> charPool,
                                     ObjectPoolProvider objectPoolProvider,
                                     MvcOptions options,
                                     MvcNewtonsoftJsonOptions jsonOptions)
            : base(logger, serializerSettings, charPool, objectPoolProvider, options, jsonOptions) {
        }

        protected override JsonSerializer CreateJsonSerializer(InputFormatterContext context) {
            var jsonProvider = (IJsonProvider) context.HttpContext.RequestServices.GetService(typeof(IJsonProvider));
            var jsonSettings = jsonProvider.GetSettings();
            var jsonSerializer = JsonSerializer.Create(jsonSettings);

            return jsonSerializer;
        }

        protected override void ReleaseJsonSerializer(JsonSerializer serializer) { }
    }
}
