using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Vacancies.Content;

public class Vacancy :  UmbracoContent {
    public DateTime ClosingDate => GetValue<Vacancy, DateTime>(x => x.ClosingDate);
}
