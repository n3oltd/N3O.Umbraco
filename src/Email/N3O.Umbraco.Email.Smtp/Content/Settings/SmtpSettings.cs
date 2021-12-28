using N3O.Umbraco.Content;

namespace N3O.Umbraco.Email.Smtp.Content {
    public class SmtpSettings : UmbracoContent {
        public string Host => GetValue<SmtpSettings, string>(x => x.Host);
        public int Port => GetValue<SmtpSettings, int>(x => x.Port);
        public string Username => GetValue<SmtpSettings, string>(x => x.Username);
        public string Password => GetValue<SmtpSettings, string>(x => x.Password);
    }
}
