using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates.Handlebars.Helpers;

public class FormatNumber : Helper {
    private readonly IFormatter _formatter;

    public FormatNumber(ILogger<FormatNumber> logger, IJsonProvider jsonProvider, IFormatter formatter)
        : base(logger, jsonProvider, 1) {
        _formatter = formatter;
    }

    protected override void Execute(EncodedTextWriter writer,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var value = args.Get<decimal?>(0);
        var output = _formatter.Number.Format(value);
        
        writer.Write(output);
    }
    
    public override string Name => "formatNumber";
}
