namespace N3O.Umbraco.Sync;

public interface IContentSyncFilter {
    bool IsFilter(string contentTypeAlias);
    bool ShouldImport(string propertyAlias);
}