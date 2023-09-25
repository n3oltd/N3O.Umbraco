using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfIsAnyOfBlockHelper : BlockHelper {
    public IfIsAnyOfBlockHelper(ILogger<IfIsAnyOfBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 2, 100) { }
    
    public override string Name => "if_is_any_of";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var arg1 = args.GetJson(0);
        var othersArgs = new List<string>();

        for (var i = 1; i < args.Count; i++) {
            othersArgs.Add(args.GetJson(i));
        }

        var areEquals = othersArgs.Any(x => AreEqual(arg1, x));
        
        if (areEquals) {
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
