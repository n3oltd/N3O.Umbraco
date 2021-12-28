using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IsBlockHelper : BlockHelper {
    public IsBlockHelper(ILogger logger, IJsonProvider jsonProvider) : base(logger, jsonProvider, 2) { }
    
    public override string Name => "is";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var val1 = args.Get<string>(0)?.ToLowerInvariant();
        var val2 = args.Get<string>(1).ToLowerInvariant();

        if (val1 == val2) {
            options.Template(output, context);
        }
    }
}
