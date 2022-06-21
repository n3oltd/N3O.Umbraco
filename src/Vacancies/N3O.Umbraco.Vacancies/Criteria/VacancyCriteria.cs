using N3O.Umbraco.Attributes;
using NodaTime;

namespace N3O.Umbraco.Vacancies.Criteria;

public class VacancyCriteria {
    [Name("Closing Date")]
    public Range<LocalDate?> ClosingDate { get; set; }
}
