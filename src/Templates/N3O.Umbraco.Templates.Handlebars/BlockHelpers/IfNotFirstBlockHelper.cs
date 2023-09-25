using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;
using System.Collections;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfNotFirstBlockHelper : BlockHelper {
    public IfNotFirstBlockHelper(ILogger<IfNotFirstBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 2) { }
    
    public override string Name => "if_not_first";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var list = args.Get<IEnumerable>(0);
        var index = args.Get<int>(1);

        if (list != null && index != 0) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }
}
