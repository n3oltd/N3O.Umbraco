using System.Collections.Generic;
using Umbraco.Cms.Infrastructure.Migrations;
using DataTypeNames = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.DataTypes;
using Shared = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.SharedDataTypes;
using States = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Migrations;

namespace N3O.Umbraco.Cloud.Platforms;

// Umbraco records the state a site has reached in umbracoKeyValue, so a step runs against a site once and
// never again. That is what makes a deletion stick: once the step that introduced a property has run, the
// property is the site's to keep or remove, and nothing puts it back
public class PlatformsSchemaPlan : MigrationPlan {
    public PlatformsSchemaPlan() : base(States.PlanName) {
        From(string.Empty).To<AddDonationPopupEmbedCodes>(States.DonationPopupEmbedCodes);
    }

    // A step that fails leaves the scope it ran in unusable for the rest of the boot, so everything that
    // touches the database afterwards fails with it, uSync's import included. The plan therefore states what
    // its steps read, and is not executed at all until a site has all of it. A new step adds whatever it
    // needs here
    public static IEnumerable<string> RequiredContentTypes {
        get {
            yield return PlatformsConstants.Campaigns.CompositionAlias;
            yield return PlatformsConstants.CrossSells.CompositionAlias;
            yield return PlatformsConstants.Offerings.CompositionAlias;
        }
    }

    public static IEnumerable<string> RequiredDataTypes {
        get {
            yield return DataTypeNames.ElementEmbedCodeLabel;
            yield return Shared.Money;
        }
    }
}
