using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfNotEqualsBlockHelper : BlockHelper {
    public IfNotEqualsBlockHelper(ILogger<IfNotEqualsBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 2) { }
    
    public override string Name => "if_not_equals";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var arg1 = args.GetJson(0);
        var arg2 = args.GetJson(1);

        if (!AreEqual(arg1, arg2)) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }

    private bool AreEqual(string arg1, string arg2) {
        if (arg1 == null && arg2 == null) {
            return true;
        }
        
        if (arg1 == null || arg2 == null) {
            return false;
        }

        return arg1.EqualsInvariant(arg2);
    }
}
