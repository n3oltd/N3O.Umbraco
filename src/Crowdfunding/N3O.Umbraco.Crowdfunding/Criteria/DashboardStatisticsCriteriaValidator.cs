using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Crowdfunding.Criteria;

public class DashboardStatisticsCriteriaValidator : ModelValidator<DashboardStatisticsCriteria> {
    public DashboardStatisticsCriteriaValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Period)
            .NotNull()
            .WithMessage(Get<Strings>(s => s.PeriodRequired));
    }

    public class Strings : ValidationStrings {
        public string PeriodRequired => "Period must be specified";
    }
}