using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class CampaignType : NamedLookup {
    public CampaignType(string id, string name) : base(id, name) { }
}

public class CampaignTypes : StaticLookupsCollection<CampaignType> {
    public static readonly CampaignType ScheduledGiving = new("scheduledGiving", "Scheduled Giving");
    public static readonly CampaignType Standard = new("standard", "Standard");
    public static readonly CampaignType Telethon = new("telethon", "Telethon");
}