using N3O.Umbraco.Extensions;
using Newtonsoft.Json;

namespace N3O.Umbraco.Email.Models {
    public class EmailIdentity : Value {
        [JsonConstructor]
        public EmailIdentity(string email, string name) {
            Email = email;
            Name = name;
        }
        
        public EmailIdentity(string email) : this(email, null) { }

        public EmailIdentity(IEmailIdentity emailIdentity) : this(emailIdentity.Email, emailIdentity.Name) { }

        public string Email { get; }
        public string Name { get; }

        public override string ToString() {
            if (!Name.HasValue()) {
                return Email;
            }

            return $"{Name} <{Email}>";
        }
    }
}