using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Models;

public class TagScope : NamedLookup {
    public TagScope(string id, string name) : base(id, name) { }
}

public class TagScopes : StaticLookupsCollection<TagScope> {
    public static readonly TagScope AccountLabel = new("accountLabel", "Account Label");
    public static readonly TagScope BeneficiaryLabel = new("beneficiaryLabel", "Beneficiary Label");
    public static readonly TagScope DonationLabel = new("donationLabel", "Donation Label");
    public static readonly TagScope FeedbackLabel = new("feedbackLabel", "Feedback Label");
    public static readonly TagScope PledgeLabel = new("pledgeLabel", "Pledge Label");
    public static readonly TagScope RegularGivingLabel = new("regularGivingLabel", "Regular Giving Label");
    public static readonly TagScope ReportLabel = new("reportLabel", "Report Label");
    public static readonly TagScope ScheduledGivingLabel = new("scheduledGivingLabel", "Scheduled Giving Label");
    public static readonly TagScope SponsorshipLabel = new("sponsorshipLabel", "Sponsorship Label");
}
