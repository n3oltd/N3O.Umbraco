using N3O.Umbraco.Content;

namespace N3O.Umbraco.Email.Smtp.Content {
    public class SmtpSettingsContent : UmbracoContent<SmtpSettingsContent> {
        public string Host => GetValue(x => x.Host);
        public int Port => GetValue(x => x.Port);
        public string Username => GetValue(x => x.Username);
        public string Password => GetValue(x => x.Password);
    }
}
