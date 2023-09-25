using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfIsOddBlockHelper : BlockHelper {
    public IfIsOddBlockHelper(ILogger<IfIsOddBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 1) { }

    public override string Name => "if_is_odd";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var val = args.Get<int>(0);

        if (val % 2 == 1) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }
}