using N3O.Umbraco.Crowdfunding.Criteria;
using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Queries;

public class GetFundraiserPagesQuery : Request<FundraiserPagesCriteria, FundraiserPageResultsPage> { }