using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Models;

public class FundraiserNotificationReqValidator : ModelValidator<FundraiserNotificationReq> {
    public FundraiserNotificationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Type)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyType));
    }

    public class Strings : ValidationStrings {
        public string SpecifyType => "Type must be specified";
    }
}