using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Accounts.Models {
    public class ConsentChoiceReqValidator : ModelValidator<ConsentChoiceReq> {
        public ConsentChoiceReqValidator(IFormatter formatter) : base(formatter) {
            RuleFor(x => x.Channel)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyChannel));
            
            RuleFor(x => x.Category)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyCategory));
            
            RuleFor(x => x.Response)
                .NotNull()
                .WithMessage(Get<Strings>(s => s.SpecifyResponse));
        }

        public class Strings : ValidationStrings {
            public string SpecifyChannel => "Please specify the channel";
            public string SpecifyCategory => "Please specify the category";
            public string SpecifyResponse => "Please specify the response";
        }
    }
}