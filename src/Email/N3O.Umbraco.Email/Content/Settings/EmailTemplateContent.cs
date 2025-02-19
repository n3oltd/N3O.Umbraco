using N3O.Umbraco.Content;

namespace N3O.Umbraco.Email.Content;

public class EmailTemplateContent<T> : UmbracoContent<T> where T : EmailTemplateContent<T> {
    public string FromName => GetValue(x => x.FromName);
    public string FromEmail => GetValue(x => x.FromEmail);
    public string BccEmail => GetValue(x => x.BccEmail);
    public string Subject => GetValue(x => x.Subject);
    public string Body => GetValue(x => x.Body);
}
