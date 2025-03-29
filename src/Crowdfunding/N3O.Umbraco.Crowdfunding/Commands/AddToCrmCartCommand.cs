using N3O.Umbraco.Crowdfunding.Models;
using N3O.Umbraco.Entities;
using N3O.Umbraco.Mediator;

namespace N3O.Umbraco.Crowdfunding.Commands;

public class AddToCrmCartCommand : Request<CrowdfundingCartReq, RevisionId> { }