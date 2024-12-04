using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;

namespace N3O.Umbraco.Elements.Content;

public class EphemeralDonationCategoryValidator : DonationCategoryValidator<EphemeralDonationCategoryContent> {
    public EphemeralDonationCategoryValidator(IContentHelper contentHelper, IContentLocator contentLocator) 
        : base(contentHelper, contentLocator) { }
    
    public override void Validate(ContentProperties content) {
        base.Validate(content);

        ValidateEndTime(content);
    }

    private void ValidateEndTime(ContentProperties content) {
        var startOn = content.GetPropertyValueByAlias<DateTime>(ElementsConstants.EphemeralDonationCategory.Properties.StartOn);
        var endOn = content.GetPropertyValueByAlias<DateTime>(ElementsConstants.EphemeralDonationCategory.Properties.EndOn);

        if (endOn < startOn) {
            ErrorResult("End date cannot be before start date");
        }
    }
}
