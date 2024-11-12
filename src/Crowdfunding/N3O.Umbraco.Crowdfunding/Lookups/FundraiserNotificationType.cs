using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class FundraiserNotificationType : NamedLookup {
    public FundraiserNotificationType(string id, string name) : base(id, name) { }
}

public class FundraiserNotificationTypes : StaticLookupsCollection<FundraiserNotificationType> {
    public static readonly FundraiserNotificationType FundraiserAbandoned = new("fundraiserAbandoned", "Fundraiser Abandoned");
    public static readonly FundraiserNotificationType FundraiserCreated = new("fundraiserCreated", "Fundraiser Created");
    public static readonly FundraiserNotificationType GoalsCompleted = new("goalsCompleted", "Goals Completed");
    public static readonly FundraiserNotificationType GoalsExceeded = new("goalsExceeded", "Goals Exceeded");
    public static readonly FundraiserNotificationType StillDraft = new("stillDraft", "Still Draft");
}