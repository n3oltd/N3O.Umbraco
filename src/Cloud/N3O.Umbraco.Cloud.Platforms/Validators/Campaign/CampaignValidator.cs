using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Validators;

public class CampaignValidator : ContentValidator {
    private static readonly string DayOfMonthAlias = AliasHelper<RegularGivingCampaignContent>.PropertyAlias(x => x.DayOfMonth);
    private static readonly string DayOfWeekAlias = AliasHelper<RegularGivingCampaignContent>.PropertyAlias(x => x.DayOfWeek);
    private static readonly string RegularGivingCampaignAlias = AliasHelper<RegularGivingCampaignContent>.ContentTypeAlias();
    private static readonly string RegularGivingFrequencyAlias = AliasHelper<RegularGivingCampaignContent>.PropertyAlias(x => x.RegularGivingFrequency);

    public CampaignValidator(IContentHelper contentHelper) : base(contentHelper) { }

    public override bool IsValidator(ContentProperties content) {
        return content.ContentTypeAlias.EqualsInvariant(RegularGivingCampaignAlias);
    }

    public override void Validate(ContentProperties content) {
        var frequencyProperty = content.Properties.Single(x => x.Alias.EqualsInvariant(RegularGivingFrequencyAlias));
        var frequency = ContentHelper.GetDataListValue<RegularGivingFrequency>(frequencyProperty);

        if (frequency == RegularGivingFrequencies.Quarterly || frequency == RegularGivingFrequencies.Annually) {
            ErrorResult(frequencyProperty, "quarterly and annual frequencies are not currently supported; please select weekly or monthly");
        }

        var dayOfWeekProperty = content.Properties.Single(x => x.Alias.EqualsInvariant(DayOfWeekAlias));
        var dayOfWeek = ContentHelper.GetDataListValue<DayOfWeek>(dayOfWeekProperty);

        var dayOfMonthProperty = content.Properties.Single(x => x.Alias.EqualsInvariant(DayOfMonthAlias));
        var dayOfMonth = ContentHelper.GetDataListValue<DayOfMonth>(dayOfMonthProperty);

        if (frequency == RegularGivingFrequencies.Monthly && dayOfWeek.HasValue()) {
            ErrorResult(dayOfWeekProperty, "cannot be specified when the frequency is monthly");
        } else if (frequency == RegularGivingFrequencies.Weekly && dayOfMonth.HasValue()) {
            ErrorResult(dayOfMonthProperty, "cannot be specified when the frequency is weekly");
        }
    }
}
