using System.Collections.Generic;
using Umbraco.Cms.Infrastructure.Migrations;
using DataTypeNames = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.DataTypes;
using Shared = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.SharedDataTypes;
using States = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Migrations;

namespace N3O.Umbraco.Cloud.Platforms;

// Umbraco records the state a site has reached in umbracoKeyValue, so a step runs against a site once and
// never again. That is what makes a deletion stick: once the step that introduced a property has run, the
// property is the site's to keep or remove
public class PlatformsSchemaPlan : MigrationPlan {
    public PlatformsSchemaPlan() : base(States.PlanName) {
        From(string.Empty).To<AddDonationPopupEmbedCodes>(States.DonationPopupEmbedCodes);
    }

    // A step that fails leaves its scope unusable for the rest of the boot, taking uSync's import with it, so
    // the plan states what its steps read and does not run until a site has all of it. A new step adds its own
    public static IReadOnlyList<string> RequiredContentTypes => [
        PlatformsConstants.Campaigns.CompositionAlias,
        PlatformsConstants.CrossSells.CompositionAlias,
        PlatformsConstants.Offerings.CompositionAlias
    ];

    public static IReadOnlyList<string> RequiredDataTypes => [
        DataTypeNames.ElementEmbedCodeLabel,
        Shared.Money
    ];
}
