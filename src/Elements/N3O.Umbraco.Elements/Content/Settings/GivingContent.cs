using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Extensions;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Elements.Content;

[UmbracoContent(ElementsConstants.Giving.Alias)]
public class GivingContent : UmbracoContent<GivingContent> {
    public Guid Id => Content().Key;
    public string CheckoutProfileId => GetValue(x => x.CheckoutProfileId);

    public IEnumerable<DonationCategoryContent> AllCategories {
        get {
            foreach (var descendant in Content().Descendants()) {
                if (descendant.IsDonationCategory()) {
                    var donationCategory = descendant.As<DonationCategoryContent>();
                        
                    yield return donationCategory;
                }
            }
        }
    }

    public IEnumerable<DonationOptionContent> AllOptions {
        get {
            foreach (var descendant in Content().Descendants()) {
                if (descendant.IsDonationOption()) {
                    var donationOption = descendant.As<DonationOptionContent>();

                    if (donationOption.IsValid()) {
                        yield return donationOption;
                    }
                }
            }
        }
    }
    
    public IEnumerable<DonationCategoryContent> RootCategories {
        get {
            var grandChildrenLevel = Content().Level + 2;

            return Content().Descendants()
                            .Where(x => x.Level == grandChildrenLevel && x.IsDonationCategory())
                            .As<DonationCategoryContent>()
                            .OrderBy(x => x.Type.Order)
                            .ToList();
        }
    }
}
