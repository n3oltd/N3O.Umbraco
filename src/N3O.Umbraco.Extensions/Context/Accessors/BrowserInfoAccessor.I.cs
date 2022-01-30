namespace N3O.Umbraco.Context {
    public interface IBrowserInfoAccessor {
        string GetAccept();
        string GetHeader(string headerName);
        string GetUserAgent();
        string GetLanguage();
    }
}