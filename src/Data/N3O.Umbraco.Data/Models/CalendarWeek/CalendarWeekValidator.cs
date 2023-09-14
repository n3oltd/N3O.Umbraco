using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models; 

public class CalendarWeekValidator : ModelValidator<CalendarWeekReq> {
    public CalendarWeekValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Week)
            .NotNull();
        
        RuleFor(x => x.Year)
            .NotNull();
        
        RuleFor(x => x.Week)
            .Must(x => x >= 1 && x <= 53)
            .When(x => x.HasValue())
            .WithMessage(Get<Strings>(s => s.WeekInvalid));

        RuleFor(x => x.Year)
            .Must(x => x >= 1900 && x <= 2100)
            .When(x => x.Year.HasValue())
            .WithMessage(Get<Strings>(s => s.YearInvalid));
    }

    public class Strings : ValidationStrings {
        public string WeekInvalid => "Week must be between 1 and 53";
        public string YearInvalid => "Year must be a valid year in the format YYYY";
    }
}
