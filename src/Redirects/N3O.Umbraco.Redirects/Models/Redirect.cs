namespace N3O.Umbraco.Redirects.Models;

public class Redirect : Value {
    public Redirect(bool temporary, string urlOrPath) {
        Temporary = temporary;
        UrlOrPath = urlOrPath;
    }

    public bool Temporary { get; }
    public string UrlOrPath { get; }
}
