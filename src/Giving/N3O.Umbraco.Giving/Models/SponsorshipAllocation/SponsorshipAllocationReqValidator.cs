using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Linq;

namespace N3O.Umbraco.Giving.Models {
    public class SponsorshipAllocationReqValidator : ModelValidator<SponsorshipAllocationReq> {
        public SponsorshipAllocationReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Scheme)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyScheme));

            RuleFor(x => x.Components)
                .Must((req, x) => x.OrEmpty().All(c => c.Component.GetScheme() == req.Scheme))
                .When(x => x.Components.All(y => y.Component.HasValue(c => c.GetScheme())))
                .WithMessage(Get<Strings>(s => s.InvalidComponents));

            RuleFor(x => x.Components)
                .Must((req, x) => req.Scheme.Components.Where(c => c.Mandatory).All(c => x.Any(y => y.Component == c)))
                .When(x => x.Scheme.OrEmpty(s => s.Components).Any(c => c.Mandatory))
                .WithMessage(Get<Strings>(s => s.MissingMandatoryComponents));
        }

        public class Strings : ValidationStrings {
            public string InvalidComponents => "One or more components are invalid";
            public string MissingMandatoryComponents => "One or more mandatory components are missing";
            public string SpecifyScheme => "Please specify the sponsorship scheme";
        }
    }
}