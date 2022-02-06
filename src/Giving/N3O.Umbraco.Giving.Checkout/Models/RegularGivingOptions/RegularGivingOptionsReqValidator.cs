using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class RegularGivingOptionsReqValidator : ModelValidator<RegularGivingOptionsReq> {
        private const int FirstCollectionMaxDaysInFuture = 180;
        
        public RegularGivingOptionsReqValidator(IFormatter formatter, ILocalClock localClock) : base(formatter) {
            RuleFor(x => x.FirstCollectionDate)
                .Must(x => x.GetValueOrThrow() >= localClock.GetLocalToday())
                .When(x => x.HasValue())
                .WithMessage(Get<Strings>(s => s.FirstCollectionInPast));
            
            RuleFor(x => x.FirstCollectionDate)
                .Must(x => (x.GetValueOrThrow() - localClock.GetLocalToday()).Days < FirstCollectionMaxDaysInFuture)
                .When(x => x.HasValue())
                .WithMessage(Get<Strings>(s => s.FirstCollectionTooFarInFuture_1, FirstCollectionMaxDaysInFuture));
        }

        public class Strings : ValidationStrings {
            public string FirstCollectionInPast => "The first collection date cannot be in the past";
            public string FirstCollectionTooFarInFuture_1 => "The first collection date cannot be more than {0} days in the future";
        }
    }
}