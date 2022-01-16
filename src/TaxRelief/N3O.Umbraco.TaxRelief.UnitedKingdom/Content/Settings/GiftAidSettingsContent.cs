using N3O.Umbraco.Content;

namespace N3O.Umbraco.TaxRelief.UnitedKingdom.Content {
    public class GiftAidSettingsContent : UmbracoContent<GiftAidSettingsContent> {
        public string Declaration => GetValue(x => x.Declaration);
    }
}
