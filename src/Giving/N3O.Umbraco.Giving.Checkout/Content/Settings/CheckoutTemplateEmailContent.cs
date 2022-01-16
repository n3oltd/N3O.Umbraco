using N3O.Umbraco.Content;

namespace N3O.Umbraco.Giving.Checkout.Content {
    public class CheckoutTemplateEmailContent : UmbracoContent<CheckoutTemplateEmailContent> {
        public string FromName => GetValue(x => x.FromName);
        public string FromEmail => GetValue(x => x.FromEmail);
        public string Subject => GetValue(x => x.Subject);
        public string Body => GetValue(x => x.Body);
    }
}