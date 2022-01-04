using N3O.Umbraco.Content;

namespace N3O.Umbraco.Giving.Checkout.Content {
    public class CheckoutTemplateEmail : UmbracoContent {
        public string FromName => GetValue<CheckoutTemplateEmail, string>(x => x.FromName);
        public string FromEmail => GetValue<CheckoutTemplateEmail, string>(x => x.FromEmail);
        public string Subject => GetValue<CheckoutTemplateEmail, string>(x => x.Subject);
        public string Body => GetValue<CheckoutTemplateEmail, string>(x => x.Body);
    }
}