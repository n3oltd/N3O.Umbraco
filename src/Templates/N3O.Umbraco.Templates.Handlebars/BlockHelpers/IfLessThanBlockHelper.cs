using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;
using Newtonsoft.Json.Linq;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfLessThanBlockHelper : BlockHelper {
    public IfLessThanBlockHelper(ILogger<IfLessThanBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 2) { }
    
    public override string Name => "if_less_than";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var collection = args.Get<JArray>(0);
        var length = args.Get<int>(1);

        if (collection.Count < length) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }
}
