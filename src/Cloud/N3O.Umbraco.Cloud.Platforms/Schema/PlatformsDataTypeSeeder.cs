using Microsoft.Extensions.Logging;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Lookups;
using N3O.Umbraco.DataTypes;
using N3O.Umbraco.Extensions;
using System;
using DataTypeNames = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.DataTypes;
using Folders = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Folders;
using QurbaniItemDataSource = N3O.Umbraco.Giving.Allocations.Lookups.QurbaniItemDataSource;

namespace N3O.Umbraco.Cloud.Platforms;

// Only the data types platforms owns are created here. Anything named without the platforms prefix belongs
// to the package that defines its data source and is referenced by name, never created
public class PlatformsDataTypeSeeder : IPlatformsDataTypeSeeder {
    private const string EmbedCodeTemplate = "<label>{{model.value}}</label>";

    private readonly IDataTypeEditor _dataTypeEditor;
    private readonly ILogger<PlatformsDataTypeSeeder> _logger;

    public PlatformsDataTypeSeeder(IDataTypeEditor dataTypeEditor, ILogger<PlatformsDataTypeSeeder> logger) {
        _dataTypeEditor = dataTypeEditor;
        _logger = logger;
    }

    // Alphabetical because nothing here depends on anything else here. A data type that names a content type
    // stores the alias as configuration and never resolves it, so these are seeded before the content types
    // that alias refers to even exist
    public void Seed() {
        Seed(DataTypeNames.AnalyticsTagsList, SeedAnalyticsTagsList);
        Seed(DataTypeNames.CampaignsMultiple, SeedCampaigns);
        Seed(DataTypeNames.DonateButtonAction, SeedDonateButtonAction);
        Seed(DataTypeNames.ECommerceStage, SeedECommerceStage);
        Seed(DataTypeNames.ElementEmbedCodeLabel, SeedElementEmbedCodeLabel);
        Seed(DataTypeNames.QurbaniItem, SeedQurbaniItem);
        Seed(DataTypeNames.QurbaniSeasonCategoryPicker, SeedQurbaniSeasonCategoryPicker);
        Seed(DataTypeNames.SuggestedAmounts, SeedSuggestedAmounts);
        Seed(DataTypeNames.Summary, SeedSummary);
    }

    // Only a data type the site does not have is created. A designer assigns the whole configuration it was
    // given, so re-seeding one the site already holds would replace every value in it with a default the site
    // never asked for; changing one is a migration step instead.
    // Umbraco does not catch what a component throws while initialising, so one that cannot be built leaves
    // the site short of that data type rather than failing the boot
    private void Seed(string name, Action seed) {
        if (_dataTypeEditor.Find(name) != null) {
            return;
        }

        try {
            seed();
        } catch (Exception ex) {
            _logger.LogError(ex, "Could not create platforms data type {Name}", name);
        }
    }

    private void SeedAnalyticsTagsList() {
        var designer = _dataTypeEditor.NewContentmentListItems(DataTypeNames.AnalyticsTagsList);

        designer.InFolder(Folders.Platforms);
        designer.WithDeterministicId(DataTypeNames.AnalyticsTagsList);

        designer.Save();
    }

    private void SeedCampaigns() {
        var designer = _dataTypeEditor.NewContentmentDataList(DataTypeNames.CampaignsMultiple);

        designer.DataSource<CampaignDataSource>();
        designer.AllowMultiple();
        designer.InFolder(Folders.Platforms);
        designer.WithDeterministicId(DataTypeNames.CampaignsMultiple);

        designer.Save();
    }

    private void SeedDonateButtonAction() {
        var designer = _dataTypeEditor.NewContentmentDataList(DataTypeNames.DonateButtonAction);

        designer.DataSource<DonationButtonActionDataSource>();
        designer.Limit(1);
        designer.InFolder(Folders.Platforms);
        designer.WithDeterministicId(DataTypeNames.DonateButtonAction);

        designer.Save();
    }

    private void SeedECommerceStage() {
        var designer = _dataTypeEditor.NewContentmentDataList(DataTypeNames.ECommerceStage);

        designer.DataSource<ECommerceStageDataSource>();
        designer.Limit(1);
        designer.InFolder(Folders.Platforms);
        designer.WithDeterministicId(DataTypeNames.ECommerceStage);

        designer.Save();
    }

    private void SeedElementEmbedCodeLabel() {
        var designer = _dataTypeEditor.NewContentmentTemplatedLabel(DataTypeNames.ElementEmbedCodeLabel);

        designer.Template(EmbedCodeTemplate);
        designer.InFolder(Folders.Platforms);
        designer.WithDeterministicId(DataTypeNames.ElementEmbedCodeLabel);

        designer.Save();
    }

    private void SeedQurbaniItem() {
        var designer = _dataTypeEditor.NewContentmentDataList(DataTypeNames.QurbaniItem);

        designer.DataSource<QurbaniItemDataSource>();
        designer.Limit(1);
        designer.InFolder(Folders.Platforms, Folders.Qurbani);
        designer.WithDeterministicId(DataTypeNames.QurbaniItem);

        designer.Save();
    }

    // The start node names a node in this site's own tree, so a picker created here is left unrooted for the
    // site to point at its own season
    private void SeedQurbaniSeasonCategoryPicker() {
        var designer = _dataTypeEditor.NewMultiNodeTreePicker(DataTypeNames.QurbaniSeasonCategoryPicker);

        designer.AllowContentTypes(PlatformsConstants.Qurbani.Season.Category.Alias);
        designer.Limit(0, 0);
        designer.InFolder(Folders.Platforms, Folders.Qurbani);
        designer.WithDeterministicId(DataTypeNames.QurbaniSeasonCategoryPicker);

        designer.Save();
    }

    private void SeedSuggestedAmounts() {
        var designer = _dataTypeEditor.NewNestedContent(DataTypeNames.SuggestedAmounts);

        designer.ElementType<DonationFormStateSuggestedAmountElement>();
        designer.Limit(0, 3);
        designer.NameTemplate("{{amount}} {{description}}");
        designer.InFolder(Folders.Platforms, Folders.DonationForms);
        designer.WithDeterministicId(DataTypeNames.SuggestedAmounts);

        designer.Save();
    }

    private void SeedSummary() {
        var designer = _dataTypeEditor.NewTextarea(DataTypeNames.Summary);

        designer.MaxChars(200);
        designer.InFolder(Folders.Platforms);
        designer.WithDeterministicId(DataTypeNames.Summary);

        designer.Save();
    }
}
