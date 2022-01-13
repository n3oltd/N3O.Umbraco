using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Donations.Content {
    public class DonationForm : UmbracoContent<DonationForm> {
        private IReadOnlyList<DonationOption> _options;

        public string Title => GetValue(x => x.Title);

        public IReadOnlyList<DonationOption> GetOptions() {
            if (_options == null) {
                var list = new List<DonationOption>();
            
                foreach (var child in Content.Children) {
                    list.Add(child.As<DonationOption>());
                }

                _options = list;
            }

            return _options;
        }
    }
}
