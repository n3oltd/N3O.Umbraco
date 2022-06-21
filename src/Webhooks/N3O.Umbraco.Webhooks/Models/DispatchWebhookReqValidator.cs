using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Webhooks.Models;

public class DispatchWebhookReqValidator : ModelValidator<DispatchWebhookReq> {
    public DispatchWebhookReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Url)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.SpecifyUrl));
    }

    public class Strings : ValidationStrings {
        public string SpecifyUrl => "URL must be specified";
    }
}
