using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Search.Typesense.NamedParameters;

public class ContentType : NamedParameter<string> {
    public override string Name => "contentType";
}