using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Entities {
    public class EntityOperation : Lookup {
        public EntityOperation(string id) : base(id) { }
    }

    public class EntityOperations : StaticLookupsCollection<EntityOperation> {
        public static readonly EntityOperation Insert = new("insert");
        public static readonly EntityOperation Delete = new("delete");
        public static readonly EntityOperation Update = new("update");
    }
}