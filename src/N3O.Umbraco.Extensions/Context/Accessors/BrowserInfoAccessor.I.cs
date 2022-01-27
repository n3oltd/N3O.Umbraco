namespace N3O.Umbraco.Context {
    public interface IBrowserInfoAccessor {
        string GetUserAgent();
        string GetLanguage();
        string GetAccept();
    }
}