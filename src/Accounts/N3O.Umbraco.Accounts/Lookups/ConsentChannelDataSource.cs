using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class ConsentChannelDataSource : LookupsDataSource<ConsentChannel> {
    public ConsentChannelDataSource(ILookups lookups) : base(lookups) { }
    
    public override string Name => "Consent Channels";
    public override string Description => "Data source for consent channels";
    public override string Icon => "icon-sensor";

    protected override string GetIcon(ConsentChannel consentChannel) => consentChannel.Icon;
}
