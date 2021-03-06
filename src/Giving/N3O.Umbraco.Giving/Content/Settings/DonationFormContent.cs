using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Giving.Content;

public class DonationFormContent : UmbracoContent<DonationFormContent> {
    private IReadOnlyList<DonationOptionContent> _options;

    public string Title => GetValue(x => x.Title);

    public IReadOnlyList<DonationOptionContent> GetOptions() {
        if (_options == null) {
            var list = new List<DonationOptionContent>();
        
            foreach (var child in Content().Children) {
                var donationOption = child.As<DonationOptionContent>();

                if (donationOption.IsValid()) {
                    list.Add(donationOption);
                }
            }

            _options = list;
        }

        return _options;
    }
}
