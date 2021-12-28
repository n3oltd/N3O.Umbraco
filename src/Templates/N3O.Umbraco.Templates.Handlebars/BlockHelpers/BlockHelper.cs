using HandlebarsDotNet;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Json;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers;

public abstract class BlockHelper : HelperBase, IBlockHelper {
    protected BlockHelper(ILogger logger, IJsonProvider jsonProvider, int args)
        : base(logger, jsonProvider, args) { }

    protected BlockHelper(ILogger logger, IJsonProvider jsonProvider, int minArgs, int maxArgs) :
        base(logger, jsonProvider, minArgs, maxArgs) { }

    public abstract string Name { get; }

    public void Execute(EncodedTextWriter output,
                        BlockHelperOptions options,
                        HandlebarsDotNet.Context context,
                        Arguments args) {
        Try(args, x => Execute(output, options, context, x));
    }

    protected abstract void Execute(EncodedTextWriter output,
                                    BlockHelperOptions options,
                                    HandlebarsDotNet.Context context,
                                    HandlebarsArguments args);
}
