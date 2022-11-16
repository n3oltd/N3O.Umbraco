using N3O.Umbraco.Entities;
using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Data.NamedParameters;

public class ExportId : NamedParameter<EntityId> {
    public override string Name => "exportId";
}
