namespace N3O.Umbraco.Context {
    public interface IQueryStringAccessor {
        string GetValue(string name);
        bool Has(string name);
    }
}
