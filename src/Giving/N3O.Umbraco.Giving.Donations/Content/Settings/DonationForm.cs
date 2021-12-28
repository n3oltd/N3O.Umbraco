using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Donations.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Giving.Donations.Content;

public class DonationForm : UmbracoContent {
    private IReadOnlyList<DonationOption> _options;

    public string Title => GetValue<DonationForm, string>(x => x.Title);

    public IReadOnlyList<DonationOption> GetOptions() {
        if (_options == null) {
            var list = new List<DonationOption>();
            
            foreach (var child in Content.Children) {
                list.Add(((PublishedContentModel) child).ToDonationOption());
            }

            _options = list;
        }

        return _options;
    }
}
