using N3O.Umbraco.Content;

namespace N3O.Umbraco.TaxRelief.UnitedKingdom.Content {
    public class GiftAidSettings : UmbracoContent<GiftAidSettings> {
        public string Declaration => GetValue(x => x.Declaration);
    }
}
