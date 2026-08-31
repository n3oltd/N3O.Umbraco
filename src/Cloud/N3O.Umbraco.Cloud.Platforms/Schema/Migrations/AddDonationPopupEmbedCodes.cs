using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Infrastructure.Migrations;
using DataTypeNames = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.DataTypes;
using Groups = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Groups;
using Shared = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.SharedDataTypes;

namespace N3O.Umbraco.Cloud.Platforms;

// The donation popup embed code is read by CampaignContent and OfferingContent and written by their sending
// handlers, but no site ever had the property, so the popup embed has been empty everywhere. Cross-sell
// amount is the same shape of gap on the sites that lack it
// Keys are stamped deterministically, the same way the seeder stamps them. A property created here without
// one would take a random key, so the same property would carry a different key in every environment that
// ran the step, and uSync matches on key
public class AddDonationPopupEmbedCodes : MigrationBase {
    private readonly IContentTypeEditor _contentTypeEditor;

    public AddDonationPopupEmbedCodes(IMigrationContext context, IContentTypeEditor contentTypeEditor)
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
