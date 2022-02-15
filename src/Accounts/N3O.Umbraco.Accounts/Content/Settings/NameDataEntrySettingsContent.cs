using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Accounts.Content {
    public class NameDataEntrySettingsContent : UmbracoContent<NameDataEntrySettingsContent> {
        public bool TitleRequired => GetValue(x => x.TitleRequired);
        public string TitleLabel => GetValue(x => x.TitleLabel);
        public string TitleHelpText => GetValue(x => x.TitleHelpText);
        public int TitleOrder => GetValue(x => x.TitleOrder);
        public IEnumerable<string> TitleOptions => GetValue(x => x.TitleOptions);
        
        public bool FirstNameRequired => GetValue(x => x.FirstNameRequired);
        public string FirstNameLabel => GetValue(x => x.FirstNameLabel);
        public string FirstNameHelpText => GetValue(x => x.FirstNameHelpText);
        public int FirstNameOrder => GetValue(x => x.FirstNameOrder);
        public Capitalisation FirstNameCapitalisation => GetValue(x => x.FirstNameCapitalisation);
        
        public bool LastNameRequired => GetValue(x => x.LastNameRequired);
        public string LastNameLabel => GetValue(x => x.LastNameLabel);
        public string LastNameHelpText => GetValue(x => x.LastNameHelpText);
        public int LastNameOrder => GetValue(x => x.LastNameOrder);
        public Capitalisation LastNameCapitalisation => GetValue(x => x.LastNameCapitalisation);

        public NameDataEntrySettings ToDataEntrySettings() {
            var title = new SelectFieldSettings(true,
                                                TitleRequired,                                                
                                                TitleLabel,
                                                TitleHelpText,
                                                HtmlField.Name<AccountReq>(x => x.Name.Title),
                                                TitleOptions.Select(x => new SelectOption(x, x)).ToList(),
                                                TitleOrder);
            var firstName = new TextFieldSettings(true,
                                                  FirstNameRequired,
                                                  FirstNameLabel,
                                                  FirstNameHelpText,
                                                  HtmlField.Name<AccountReq>(x => x.Name.FirstName),
                                                  FirstNameOrder,
                                                  false,
                                                  FirstNameCapitalisation);
            var lastName = new TextFieldSettings(true,
                                                 LastNameRequired,
                                                 LastNameLabel,
                                                 LastNameHelpText,
                                                 HtmlField.Name<AccountReq>(x => x.Name.LastName),
                                                 LastNameOrder,
                                                 false,
                                                 LastNameCapitalisation);

            return new NameDataEntrySettings(title, firstName, lastName);
        }
    }
}
