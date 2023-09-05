using FluentValidation;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Data.Models; 

public class CalendarMonthValidator : ModelValidator<CalendarMonthReq> {
    public CalendarMonthValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Month);
        RuleFor(x => x.Year);
        
        RuleFor(x => x.Month)
            .Must(x => x >= 1 && x <= 12)
            .When(x => x.HasValue())
            .WithMessage(Get<Strings>(s => s.MonthInvalid));

        RuleFor(x => x.Year)
            .Must(x => x >= 1900 && x <= 2100)
            .When(x => x.Year.HasValue())
            .WithMessage(Get<Strings>(s => s.YearInvalid));
    }

    public class Strings : ValidationStrings {
        public string MonthInvalid => "Month must be between 1 and 12";
        public string YearInvalid => "Year must be a valid year in the format YYYY";
    }
}
