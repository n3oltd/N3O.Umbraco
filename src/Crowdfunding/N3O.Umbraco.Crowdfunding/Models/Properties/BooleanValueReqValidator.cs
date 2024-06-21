using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class BooleanValueReqValidator : ModelValidator<BooleanValueReq> {
    // TODO Ensure value is not null
    public BooleanValueReqValidator(IFormatter formatter) : base(formatter) { }
}