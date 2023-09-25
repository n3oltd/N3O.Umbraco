using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;
using System.Collections;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfHasAnyBlockHelper : BlockHelper {
    public IfHasAnyBlockHelper(ILogger<IfHasAnyBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 1) { }
    
    public override string Name => "if_has_any";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var list = args.Get<IEnumerable>(0);

        if (list != null && list.GetEnumerator().MoveNext()) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }
}
