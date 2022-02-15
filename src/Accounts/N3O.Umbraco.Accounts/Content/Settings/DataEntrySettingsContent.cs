using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Accounts.Content {
    public class DataEntrySettingsContent : UmbracoContent<DataEntrySettingsContent> {
        public string HeadHtml => GetValue(x => x.HeadHtml);
        public string BodyHtml => GetValue(x => x.BodyHtml);
        public string Css => GetValue(x => x.Css);
        public string JavaScript => GetValue(x => x.JavaScript);
        public NameDataEntrySettingsContent Name => Child(x => x.Name);
        public AddressDataEntrySettingsContent Address => Child(x => x.Address);
        public EmailDataEntrySettingsContent Email => Child(x => x.Email);
        public PhoneDataEntrySettingsContent Phone => Child(x => x.Phone);

        public DataEntrySettings ToDataEntrySettings(ILookups lookups, IEnumerable<ConsentOptionContent> consentOptions) {
            var consentSettings = new ConsentDataEntrySettings(consentOptions.OrEmpty()
                                                                             .Select(x => x.ToConsentOption()));
            
            return new DataEntrySettings(Name.ToDataEntrySettings(),
                                         Address.ToDataEntrySettings(lookups),
                                         Email.ToDataEntrySettings(),
                                         Phone.ToDataEntrySettings(lookups),
                                         consentSettings);
        }
    }
}