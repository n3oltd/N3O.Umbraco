using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfHasValueBlockHelper : BlockHelper {
    public IfHasValueBlockHelper(ILogger<IfHasValueBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 1) { }
    
    public override string Name => "if_has_value";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var obj = args.Get<object>(0);

        if (obj.HasValue()) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }
}
