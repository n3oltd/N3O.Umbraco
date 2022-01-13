using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Vacancies.Content {
    public class Vacancy : UmbracoContent<Vacancy> {
        public DateTime ClosingDate => GetValue(x => x.ClosingDate);
    }
}