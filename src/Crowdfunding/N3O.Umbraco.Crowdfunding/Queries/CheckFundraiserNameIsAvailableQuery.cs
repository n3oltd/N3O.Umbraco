using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Queries;

[NoValidation]
public class CheckFundraiserNameIsAvailableQuery : Request<CreateFundraiserReq, bool> { }