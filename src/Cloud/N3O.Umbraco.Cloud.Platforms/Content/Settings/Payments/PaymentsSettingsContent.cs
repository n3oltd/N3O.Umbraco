using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Payments.Alias)]
public class PaymentsSettingsContent : UmbracoContent<PaymentsSettingsContent> {
    public BannerAdvertsContent BannerAdverts => Content().GetSingleChildOfTypeAs<BannerAdvertsContent>();
    public PaymentTermsContent PaymentTerms => Content().GetSingleChildOfTypeAs<PaymentTermsContent>();
}