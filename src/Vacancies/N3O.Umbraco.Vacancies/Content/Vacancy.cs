using N3O.Umbraco.Content;
using NodaTime;

namespace N3O.Umbraco.Vacancies.Content {
    public class Vacancy : UmbracoContent<Vacancy> {
        public LocalDate ClosingDate => GetLocalDate(x => x.ClosingDate);
    }
}