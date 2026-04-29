using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public interface IHoldDonationFormContent {
    string Summary { get; }
    IHtmlEncodedString Description { get; }
    MediaWithCrops Image { get; }
    MediaWithCrops Icon { get; }
}
