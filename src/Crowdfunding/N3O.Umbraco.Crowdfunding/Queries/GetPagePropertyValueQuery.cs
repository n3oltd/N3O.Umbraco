using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Queries;

public class GetPagePropertyValueQuery : Request<None, PagePropertyValueRes> {
    public PageId PageId { get; }
    public PropertyAlias PropertyAlias { get; }

    public GetPagePropertyValueQuery(PageId pageId, PropertyAlias propertyAlias) {
        PageId = pageId;
        PropertyAlias = propertyAlias;
    }
}