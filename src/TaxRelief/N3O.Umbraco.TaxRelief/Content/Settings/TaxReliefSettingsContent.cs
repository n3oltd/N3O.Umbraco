using N3O.Umbraco.Content;
using N3O.Umbraco.TaxRelief.Lookups;

namespace N3O.Umbraco.TaxRelief.Content;

public class TaxReliefSettingsContent : UmbracoContent<TaxReliefSettingsContent> {
    public TaxReliefScheme Scheme => GetValue(x => x.Scheme);
}
