namespace N3O.Umbraco.Sync;

public interface ISyncFilter {
    bool IsFilter(string contentTypeAlias);
    bool ShouldImport(string propertyAlias);
}