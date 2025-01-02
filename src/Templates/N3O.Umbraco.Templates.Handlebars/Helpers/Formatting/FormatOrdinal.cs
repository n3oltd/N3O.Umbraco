using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Json;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates.Handlebars.Helpers;

public class FormatOrdinal : Helper {
    private readonly IFormatter _formatter;

    public FormatOrdinal(ILogger<FormatOrdinal> logger, IJsonProvider jsonProvider, IFormatter formatter)
        : base(logger, jsonProvider, 1) {
        _formatter = formatter;
    }

    protected override void Execute(EncodedTextWriter writer,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var value = args.Get<int?>(0);
        var output = value.IfNotNull(x => _formatter.Number.FormatOrdinal(x));
        
        writer.Write(output);
    }
    
    public override string Name => "formatOrdinal";
}
