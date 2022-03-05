using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates.Handlebars.Helpers {
    public class FormatMoney : Helper {
        private readonly IFormatter _formatter;

        public FormatMoney(ILogger<FormatMoney> logger, IJsonProvider jsonProvider, IFormatter formatter)
            : base(logger, jsonProvider, 1) {
            _formatter = formatter;
        }

        protected override void Execute(EncodedTextWriter writer,
                                        HandlebarsDotNet.Context context,
                                        HandlebarsArguments args) {
            var value = args.Get<Money>(0);
            var output = _formatter.Number.FormatMoney(value);
            
            writer.Write(output);
        }
        
        public override string Name => "formatMoney";
    }
}