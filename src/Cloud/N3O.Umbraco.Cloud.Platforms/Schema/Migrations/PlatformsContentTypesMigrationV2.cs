using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Infrastructure.Migrations;
using DataTypeNames = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.DataTypes;
using Groups = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Groups;
using Shared = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.SharedDataTypes;

namespace N3O.Umbraco.Cloud.Platforms;

// The donation popup embed code is read and written in code but no site ever had the property, so the popup
// embed has been empty everywhere; cross-sell amount is the same gap on the sites that lack it.
// Keys are stamped deterministically, as the seeder stamps them: without that the same property would carry
// a different key in every environment that ran the step, and uSync matches on key
public class PlatformsContentTypesMigrationV2 : MigrationBase {
    private readonly IContentTypeEditor _contentTypeEditor;

    public PlatformsContentTypesMigrationV2(IMigrationContext context, IContentTypeEditor contentTypeEditor)
        : base(context) {
        _contentTypeEditor = contentTypeEditor;
    }

    protected override void Migrate() {
        AddCampaignPopupEmbedCode();
        AddCrossSellAmount();
        AddOfferingPopupEmbedCode();
    }

    private void AddCampaignPopupEmbedCode() {
        var designer = _contentTypeEditor.NewDocument<CampaignContent>();

        designer.WithDeterministicId();

        designer.Group(Groups.Embed)
                .ContentmentTemplatedLabel(x => x.DonationPopupEmbedCode)
                .DataType(DataTypeNames.ElementEmbedCodeLabel);

        designer.Save();
    }

    private void AddCrossSellAmount() {
        var designer = _contentTypeEditor.NewDocument<CrossSellContent>();

        designer.WithDeterministicId();

        designer.Group(Groups.General).Decimal(x => x.Amount).DataType(Shared.Money);

        designer.Save();
    }

    private void AddOfferingPopupEmbedCode() {
        var designer = _contentTypeEditor.NewDocument<OfferingContent>();

        designer.WithDeterministicId();

        designer.Group(Groups.Embed)
                .ContentmentTemplatedLabel(x => x.DonationPopupEmbedCode)
                .DataType(DataTypeNames.ElementEmbedCodeLabel);

        designer.Save();
    }
}
