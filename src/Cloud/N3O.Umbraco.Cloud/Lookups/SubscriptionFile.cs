using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Lookups;

public class SubscriptionFile : Lookup {
    public SubscriptionFile(string id) : base(id) { }

    public string Filename => $"{Id}.json";
}

public class SubscriptionFiles : StaticLookupsCollection<SubscriptionFile> {
    public static readonly SubscriptionFile AccountEntry = new("accountEntry");
    public static readonly SubscriptionFile Currencies = new("currencies");
    public static readonly SubscriptionFile DonateMenu = new("donateMenu");
    public static readonly SubscriptionFile DonationItems = new("donationItems");
    public static readonly SubscriptionFile Environment = new("environment");
    public static readonly SubscriptionFile FundStructure = new("fundStructure");
    public static readonly SubscriptionFile Localization = new("localization");
    public static readonly SubscriptionFile Lookups = new("lookups");
    public static readonly SubscriptionFile Nisab = new("nisab");
    public static readonly SubscriptionFile OrganizationInfo = new("organizationInfo");
    public static readonly SubscriptionFile Payments = new("payments");
    public static readonly SubscriptionFile Tracking = new("tracking");
}