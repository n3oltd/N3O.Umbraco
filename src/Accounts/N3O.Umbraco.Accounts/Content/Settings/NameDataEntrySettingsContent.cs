using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

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
            var title = new TitleDataEntrySettings(TitleRequired, TitleLabel, TitleHelpText, TitleOrder, TitleOptions);
            var firstName = new FirstNameDataEntrySettings(FirstNameRequired,
                                                           FirstNameLabel,
                                                           FirstNameHelpText,
                                                           FirstNameOrder,
                                                           FirstNameCapitalisation);
            var lastName = new LastNameDataEntrySettings(LastNameRequired,
                                                         LastNameLabel,
                                                         LastNameHelpText,
                                                         LastNameOrder,
                                                         LastNameCapitalisation);

            return new NameDataEntrySettings(title, firstName, lastName);
        }
    }
}
