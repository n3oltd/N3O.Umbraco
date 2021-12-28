using N3O.Umbraco.Content;

namespace N3O.Umbraco.TaxRelief.UnitedKingdom.Content;

public class GiftAidSettings : UmbracoContent {
    public string Declaration => GetValue<GiftAidSettings, string>(x => x.Declaration);
}
