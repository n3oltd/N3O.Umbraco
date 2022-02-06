using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;
using N3O.Umbraco.Utilities;

namespace N3O.Umbraco.Templates.Handlebars.Helpers {
    public class AbsoluteUrlHelper : Helper {
        private readonly IUrlBuilder _urlBuilder;

        public AbsoluteUrlHelper(ILogger<AbsoluteUrlHelper> logger, IJsonProvider jsonProvider, IUrlBuilder urlBuilder)
            : base(logger, jsonProvider, 1) {
            _urlBuilder = urlBuilder;
        }

        protected override void Execute(EncodedTextWriter writer,
                                        HandlebarsDotNet.Context context,
                                        HandlebarsArguments args) {
            var path = args.Get<string>(0);
            var absoluteUrl = _urlBuilder.Root().AppendPathSegment(path);
            
            writer.Write(absoluteUrl.ToString());
        }
        
        public override string Name => "absoluteUrl";
    }
}