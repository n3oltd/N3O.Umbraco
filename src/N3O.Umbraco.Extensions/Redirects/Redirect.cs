namespace N3O.Umbraco.Redirects;

public class Redirect : Value {
    public Redirect(string urlOrPath, bool temporary) {
        UrlOrPath = urlOrPath;
        Temporary = temporary;
    }

    public string UrlOrPath { get; }
    public bool Temporary { get; }
}
