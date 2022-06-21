using HandlebarsDotNet;

namespace N3O.Umbraco.Templates.Handlebars.Helpers;

public interface IHelper {
    string Name { get; }

    void Execute(EncodedTextWriter writer, HandlebarsDotNet.Context context, Arguments args);
}
