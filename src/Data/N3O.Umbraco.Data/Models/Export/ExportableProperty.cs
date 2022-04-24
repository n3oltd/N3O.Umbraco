namespace N3O.Umbraco.Data.Models {
    public class ExportableProperty : Value {
        public ExportableProperty(string alias, string name) {
            Alias = alias;
            Name = name;
        }

        public string Alias { get; }
        public string Name { get; }
    }
}