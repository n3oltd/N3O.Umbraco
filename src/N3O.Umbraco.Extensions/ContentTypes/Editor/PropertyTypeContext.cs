namespace N3O.Umbraco.ContentTypes;

public class PropertyTypeContext {
    public PropertyTypeContext(string contentTypeAlias, string propertyAlias, bool useDeterministicIds) {
        ContentTypeAlias = contentTypeAlias;
        PropertyAlias = propertyAlias;
        UseDeterministicIds = useDeterministicIds;
    }

    public string ContentTypeAlias { get; }
    public string PropertyAlias { get; }
    public bool UseDeterministicIds { get; }
}
