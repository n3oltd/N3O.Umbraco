using System.Collections.Generic;
using Umbraco.Cms.Infrastructure.Migrations;
using DataTypeNames = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.DataTypes;
using Shared = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.SharedDataTypes;
using States = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Migrations;

namespace N3O.Umbraco.Cloud.Platforms;

public class PlatformsSchemaPlan : MigrationPlan {
    public PlatformsSchemaPlan() : base(States.PlanName) {
        From(string.Empty).To<PlatformsContentTypesMigrationV2>(States.PlatformsContentTypesMigrationV2);
    }

    // What the steps read, so the plan can be left alone until a site has all of it. These arrive by uSync,
    // whose import is manual outside development, and a plan run before it would fail on every boot until it
    // ran. A new step adds whatever it reads here
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
