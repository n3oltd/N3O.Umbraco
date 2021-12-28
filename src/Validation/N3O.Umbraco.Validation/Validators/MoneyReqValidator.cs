using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Validation.Validators {
    public class MoneyReqValidator : ModelValidator<MoneyReq> {
        public MoneyReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Amount)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyAmount));

            RuleFor(x => x.Currency)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyCurrency));
        
            RuleFor(x => x.Amount)
                .Must(x => x >= 1m && Math.Round(x.Value, 2, MidpointRounding.AwayFromZero) == x && x < 100000)
                .When(x => x.Amount.HasValue())
                .WithMessage(Get<Strings>(s => s.InvalidAmount));
        }

        public class Strings : ValidationStrings {
            public string InvalidAmount => "Amount is invalid";
            public string SpecifyAmount => "Please specify an amount";
            public string SpecifyCurrency => "Please specify a currency";
        }
    }
}