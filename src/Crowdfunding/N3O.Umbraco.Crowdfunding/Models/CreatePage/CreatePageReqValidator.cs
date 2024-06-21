using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class CreatePageReqValidator : ModelValidator<CreatePageReq> {
    // TODO Validate length of page name and check it for profanity. We also need to check that the page name
    // is not already in use. Also need to check that the campaign ID is specified, and valid etc. Avoid any
    // duplicate logic between this file and CrowdfundingDataReqValidator.
    public CreatePageReqValidator(IFormatter formatter) : base(formatter) { }
}