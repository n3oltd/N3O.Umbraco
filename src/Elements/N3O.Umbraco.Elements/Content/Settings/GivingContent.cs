using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Content;

public class GivingContent : UmbracoContent<GivingContent> {
    private IReadOnlyList<DonationOptionContent> _options;
    private IReadOnlyList<DonationCategoryBaseContent> _categories;

    public string Title => GetValue(x => x.Title);
    
    public IReadOnlyList<DonationCategoryBaseContent> GetDonationCategories() {
        if (_options == null) {
            var list = new List<DonationCategoryBaseContent>();
        
            foreach (var descendant in Content().Descendants()) {
                if (descendant.IsDonationCategory()) {
                    var donationCategory = descendant.As<DonationCategoryBaseContent>();
                    
                    list.Add(donationCategory);
                }
            }

            _categories = list;
        }

        return _categories;
    }

    public IReadOnlyList<DonationOptionContent> GetDonationOptions() {
        if (_options == null) {
            var list = new List<DonationOptionContent>();
        
            foreach (var descendant in Content().Descendants()) {
                if (descendant.ContentType.CompositionAliases.Contains(ElementsConstants.DonationOption.Alias)) {
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

    public object GetFormJson(IJsonProvider jsonProvider) {
        var option = new {
            FormId = Content().Key
        };
        
        return jsonProvider.SerializeObject(option);
    }
}
