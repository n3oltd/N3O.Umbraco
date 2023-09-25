using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;
using System.Collections;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public class IfNotLastBlockHelper : BlockHelper {
    public IfNotLastBlockHelper(ILogger<IfNotLastBlockHelper> logger, IJsonProvider jsonProvider)
        : base(logger, jsonProvider, 2) { }
    
    public override string Name => "if_not_last";

    protected override void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args) {
        var list = args.Get<IEnumerable>(0);
        var index = args.Get<int>(1);

        if (list != null && (index + 1) != Count(list)) {
            options.Template(output, context);
        } else {
            options.Inverse(output, context);
        }
    }

    private int Count(IEnumerable list) {
        var count = 0;

        var enumerator = list.GetEnumerator();
                
        while (enumerator.MoveNext()) {
            count++;
        }

        return count;
    }
}
