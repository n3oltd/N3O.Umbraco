namespace N3O.Umbraco.Context {
    public interface ICurrentUrlAccessor {
        string GetDisplayUrl();
        string GetEncodedUrl();
    }
}
