using N3O.Umbraco.Parameters;
using System;

namespace N3O.Umbraco.Crowdfunding.NamedParameters; 

public class PageId : NamedParameter<Guid> {
    public override string Name => "pageId";
}