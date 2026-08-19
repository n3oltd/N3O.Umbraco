namespace N3O.Umbraco.Context;

public interface ICultureAccessor {
    string GetCulture(string culture = null);
}
