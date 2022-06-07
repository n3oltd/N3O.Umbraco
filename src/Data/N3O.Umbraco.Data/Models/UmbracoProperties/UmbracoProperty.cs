namespace N3O.Umbraco.Data.Models {
    public class UmbracoPropertyRes {
        public UmbracoPropertyRes(string name, string alias, string group, string dataType) {
            Name = name;
            Alias = alias;
            Group = group;
            DataType = dataType;
        }

        public string Name { get; }
        public string Alias { get; }
        public string Group { get; }
        public string DataType { get; }
    }
}