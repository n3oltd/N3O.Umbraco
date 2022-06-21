namespace N3O.Umbraco.Data.Models;

public class ExportableProperty : Value {
    public ExportableProperty(string alias, string columnTitle) {
        Alias = alias;
        ColumnTitle = columnTitle;
    }

    public string Alias { get; }
    public string ColumnTitle { get; }
}
