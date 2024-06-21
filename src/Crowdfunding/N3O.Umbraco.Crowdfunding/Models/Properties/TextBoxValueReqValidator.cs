using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class TextBoxValueReqValidator : ModelValidator<TextBoxValueReq> {
    // TODO Ensure value is not null
    public TextBoxValueReqValidator(IFormatter formatter) : base(formatter) { }
}