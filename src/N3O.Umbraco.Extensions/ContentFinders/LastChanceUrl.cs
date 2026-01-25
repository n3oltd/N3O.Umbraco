namespace N3O.Umbraco.ContentFinders;

public class LastChanceUrl : Value {
    public LastChanceUrl(string path, bool temporary) {
        Path = path;
        Temporary = temporary;
    }
    
    public string Path { get; }
    public bool Temporary { get; }
}
