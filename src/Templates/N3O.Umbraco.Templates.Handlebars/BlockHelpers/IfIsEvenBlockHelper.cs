using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfIsEvenBlockHelper : BlockHelper {
    public IfIsEvenBlockHelper(ILogger<IfIsEvenBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 1) { }
    
    public override string Name => "if_is_even";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var val = args.Get<int>(0);

        if (val % 2 == 0) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }
}