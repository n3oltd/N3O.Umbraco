﻿using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Crowdfunding.NamedParameters;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Queries;

public class GetFundraiserGoalsQuery : Request<None, FundraiserGoalsRes> {
    public ContentId ContentId { get; }

    public GetFundraiserGoalsQuery(ContentId contentId) {
        ContentId = contentId;
    }
}