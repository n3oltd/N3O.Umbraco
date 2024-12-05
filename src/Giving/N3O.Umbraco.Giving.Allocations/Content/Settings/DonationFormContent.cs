using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Allocations.Content;

public class DonationFormContent : UmbracoContent<DonationFormContent> {
    private IReadOnlyList<DonationOptionContent> _options;

    public string Title => GetValue(x => x.Title);

    public IReadOnlyList<DonationOptionContent> GetOptions() {
        if (_options == null) {
            var list = new List<DonationOptionContent>();
        
            foreach (var descendant in Content().Descendants()) {
                if (descendant.ContentType.CompositionAliases.Contains(AllocationsConstants.Aliases.DonationOption.ContentType)) {
                    var donationOption = descendant.As<DonationOptionContent>();

                    if (donationOption.IsValid()) {
                        list.Add(donationOption);
                    }
                }
            }

            _options = list;
        }

        return _options;
    }
}
