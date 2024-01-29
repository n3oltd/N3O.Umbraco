using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class ConsentChannel : NamedLookup {
    public ConsentChannel(string id, string name, string icon) : base(id, name) {
        Icon = icon;
    }
    
    public string Icon { get; }
}

public class Channels : StaticLookupsCollection<ConsentChannel> {
    public static ConsentChannel Email = new("email", "Email", "icon-keyboard");
    public static ConsentChannel Sms = new("sms", "SMS", "icon-mobile");
    public static ConsentChannel Post = new("post", "Post", "icon-message-unopened");
    public static ConsentChannel Telephone = new("telephone", "Telephone", "icon-phone");
    public static ConsentChannel WhatsApp = new("whatsApp", "WhatsApp", "icon-phone");
}
