using HandlebarsDotNet;

namespace N3O.Umbraco.Templates.Handlebars.BlockHelpers {
    public interface IBlockHelper {
        string Name { get; }

        void Execute(EncodedTextWriter output, BlockHelperOptions options, HandlebarsDotNet.Context context, Arguments args);
    }
}
