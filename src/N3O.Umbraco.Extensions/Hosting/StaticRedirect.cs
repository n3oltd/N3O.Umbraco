namespace N3O.Umbraco.Hosting;

public class StaticRedirect : Value {
    public StaticRedirect(string path, bool temporary) {
        Path = path;
        Temporary = temporary;
    }
    
    public string Path { get; }
    public bool Temporary { get; }
}
