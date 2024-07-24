using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class GetPropertyValueCommand : Request<None, PagePropertyValueRes> {
    public PropertyAlias PropertyAlias { get; }
    public PageId PageId { get; }

    public GetPropertyValueCommand(PropertyAlias propertyAlias, PageId pageId) {
        PropertyAlias = propertyAlias;
        PageId = pageId;
    }
}