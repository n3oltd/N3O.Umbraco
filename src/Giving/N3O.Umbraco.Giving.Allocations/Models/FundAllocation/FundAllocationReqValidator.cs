using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FundAllocationReqValidator : ModelValidator<FundAllocationReq> {
    public FundAllocationReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.DonationItem)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyDonationItem));
    }

    public class Strings : ValidationStrings {
        public string SpecifyDonationItem => "Please specify the donation item";
    }
}
