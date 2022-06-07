namespace N3O.Umbraco.Data.Models {
    public class ImportField {
        public string Property { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string SourceValue { get; set; }
        public bool IsFile { get; set; }
    }
}