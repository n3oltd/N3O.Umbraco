using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfShorterThanBlockHelper : BlockHelper {
    public IfShorterThanBlockHelper(ILogger<IfShorterThanBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 2) { }
    
    public override string Name => "if_shorter_than";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var text = args.Get<string>(0);
        var minLength = args.Get<int>(1);

        if (text.HasValue() && text.Length < minLength) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }
}
