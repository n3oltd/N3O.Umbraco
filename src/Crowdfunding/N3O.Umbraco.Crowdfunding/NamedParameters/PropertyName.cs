using N3O.Umbraco.Parameters;

namespace N3O.Umbraco.Crowdfunding.NamedParameters; 

public class PropertyName : NamedParameter<string> {
    public override string Name => "propertyName";
}