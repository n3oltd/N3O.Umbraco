using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Content;

[UmbracoContent(ElementsConstants.ElementsCheckoutCompleteSettings.Alias)]
public class CheckoutCompleteSettingsContent : UmbracoContent<CheckoutCompleteSettingsContent> {
    public IEnumerable<AdvertContentElement> Adverts => GetNestedAs(x => x.Adverts);
}