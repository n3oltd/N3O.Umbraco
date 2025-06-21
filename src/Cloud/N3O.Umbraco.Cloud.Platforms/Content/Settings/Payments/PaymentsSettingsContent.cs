using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Payments.Alias)]
public class PaymentsSettingsContent : UmbracoContent<PaymentsSettingsContent> {
    public BannerAdvertsContent BannerAdverts => Content().GetSingleChildOfTypeAs<BannerAdvertsContent>();
    public PaymentTermsContent Terms => Content().GetSingleChildOfTypeAs<PaymentTermsContent>();
}