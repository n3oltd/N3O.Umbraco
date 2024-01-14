using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Crowdfunding.NamedParameters; 

public class PropertyId : NamedParameter<int> {
    public override string Name => "propertyId";
}