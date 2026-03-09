namespace N3O.Umbraco.Search.Typesense;

public static class TypesenseConstants {
    public const string BackOfficeApiName = "TypesenseDevTools";

    public static class Configuration {
        public static readonly string Section = "Typesense";
    }
    
    public static class MetadataKeys {
        public static readonly string Version = nameof(Version);
    }
}