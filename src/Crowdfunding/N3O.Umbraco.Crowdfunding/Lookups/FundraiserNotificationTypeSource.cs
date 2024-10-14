using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Lookups;

public class FundraiserNotificationTypeSource : LookupsDataSource<FundraiserNotificationType> {
    public FundraiserNotificationTypeSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Fundraiser Notification Type";
    public override string Description => "Data source for fundraiser notification types";
    public override string Icon => "icon-message";

    protected override string GetIcon(FundraiserNotificationType fundraiserNotificationType) => "icon-message";
}
