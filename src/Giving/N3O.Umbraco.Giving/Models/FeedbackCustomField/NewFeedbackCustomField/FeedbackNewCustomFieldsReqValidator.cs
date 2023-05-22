using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackNewCustomFieldsReqValidator : ModelValidator<FeedbackNewCustomFieldsReq> {
    public FeedbackNewCustomFieldsReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Entries)
           .NotNull()
           .Must(AllCustomFieldAliasesAreUnique)
           .WithMessage(Get<Strings>(s => s.CustomFieldAliasesMustBeUnique));
    }

    private bool AllCustomFieldAliasesAreUnique(IEnumerable<FeedbackNewCustomFieldReq> newCustomFields) {
        return newCustomFields.Select(x => x.Alias).Distinct().Count() == newCustomFields.Count();
    }

    public class Strings : ValidationStrings {
        public string CustomFieldAliasesMustBeUnique => "Custom fields must be unique";
    }
}