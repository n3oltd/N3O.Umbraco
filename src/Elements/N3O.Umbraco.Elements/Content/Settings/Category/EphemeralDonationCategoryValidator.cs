using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Elements.Content;

public class EphemeralDonationCategoryValidator : DonationCategoryValidator<EphemeralDonationCategoryContent> {
    public EphemeralDonationCategoryValidator(IContentHelper contentHelper) : base(contentHelper) { }

    protected override void ValidateCategory(ContentProperties content) {
        var startOn = content.GetPropertyValueByAlias<DateTime>(ElementsConstants.DonationCategory.Ephemeral.Properties.StartOn);
        var endOn = content.GetPropertyValueByAlias<DateTime>(ElementsConstants.DonationCategory.Ephemeral.Properties.EndOn);

        if (startOn > endOn) {
            ErrorResult("End date must be after start date");
        }
    }
}
