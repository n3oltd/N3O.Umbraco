using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.PaymentTerms.Alias)]
public class PaymentTermsContent : UmbracoContent<PaymentTermsContent> {
    public Link Link => GetValue(x => x.Link);
    public string Text => GetValue(x => x.Text);
}