namespace N3O.Umbraco.Localization;

public interface IStringLocalizer {
    string Get(string folder, string name, string text);
}
