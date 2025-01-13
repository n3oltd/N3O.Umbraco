using N3O.Umbraco.Attributes;
using NodaTime;

namespace N3O.Umbraco.Crowdfunding.Criteria;

public class DashboardStatisticsCriteria {
    [Name("Period")]
    public Range<LocalDate?> Period { get; set; }
}