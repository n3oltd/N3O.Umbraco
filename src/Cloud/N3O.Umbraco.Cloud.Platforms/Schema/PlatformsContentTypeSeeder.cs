using Microsoft.Extensions.Logging;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms;

public partial class PlatformsContentTypeSeeder : IPlatformsContentTypeSeeder {
    private readonly IContentTypeEditor _contentTypeEditor;
    private readonly IContentTypeService _contentTypeService;
    private readonly IDataTypeService _dataTypeService;
    private readonly ILogger<PlatformsContentTypeSeeder> _logger;

    public PlatformsContentTypeSeeder(IContentTypeEditor contentTypeEditor,
                                      IContentTypeService contentTypeService,
                                      IDataTypeService dataTypeService,
                                      ILogger<PlatformsContentTypeSeeder> logger) {
        _contentTypeEditor = contentTypeEditor;
        _contentTypeService = contentTypeService;
        _dataTypeService = dataTypeService;
        _logger = logger;
    }

    // A subscription defines however many fund dimensions it uses, so the data type behind a higher
    // dimension legitimately does not exist on every site and that property is simply not part of its schema
    private bool HasDataType(string name) {
        return _dataTypeService.GetDataType(name) != null;
    }

    // Elements first because the document types compose them, and compositions resolve by alias on save
    public void Seed() {
        Seed(PlatformsConstants.DonationFormState.SuggestedAmount, SeedSuggestedAmount);
        Seed(PlatformsConstants.DonationFormContent.CompositionAlias, SeedDonationFormContent);
        Seed(PlatformsConstants.DonationFormState.CompositionAlias, SeedDonationFormState);
        Seed(PlatformsConstants.DonationFormState.Feedback, SeedFeedbackDonationFormState);
        Seed(PlatformsConstants.DonationFormState.Fund, SeedFundDonationFormState);
        Seed(PlatformsConstants.DonationFormState.Qurbani, SeedQurbaniDonationFormState);
        Seed(PlatformsConstants.DonationFormState.Sponsorship, SeedSponsorshipDonationFormState);

        Seed(PlatformsConstants.Platforms.Alias, SeedPlatforms);

        Seed(PlatformsConstants.Campaigns.CompositionAlias, SeedCampaign);
        Seed(PlatformsConstants.Campaigns.Standard, SeedStandardCampaign);
        Seed(PlatformsConstants.Campaigns.Giving, SeedGivingCampaign);
        Seed(PlatformsConstants.Campaigns.Qurbani, SeedQurbaniCampaign);
        Seed(PlatformsConstants.Campaigns.Telethon, SeedTelethonCampaign);
        Seed(PlatformsConstants.Campaigns.ScheduledGiving, SeedScheduledGivingCampaign);
        Seed(PlatformsConstants.Campaigns.RegularGiving, SeedRegularGivingCampaign);
        Seed(PlatformsConstants.Offerings.CompositionAlias, SeedOffering);
        Seed(PlatformsConstants.CrossSells.CompositionAlias, SeedCrossSell);
        Seed(PlatformsConstants.Qurbani.Season.Category.Alias, SeedQurbaniSeasonCategory);
        Seed(PlatformsConstants.Qurbani.Season.Alias, SeedQurbaniSeason);

        // A parent names its children as allowed, and an allowed child is resolved when the parent is saved,
        // so a child seeded after its parent leaves the parent for the next boot. Seeded leaf upwards, the
        // whole calculator arrives in one, the same way a season category is seeded before its season
        Seed(PlatformsConstants.Zakat.Settings.Calculator.Field.Alias, SeedZakatCalculatorField);
        Seed(PlatformsConstants.Zakat.Settings.Calculator.Section.Alias, SeedZakatCalculatorSection);
        Seed(PlatformsConstants.Zakat.Settings.Calculator.Alias, SeedZakatCalculatorSettings);
    }

    // Only a type the site does not have is created. A type it already holds is left exactly as its editors
    // have shaped it, and is changed only by a migration step, so a property a site has deliberately removed
    // is never put back.
    // Every type is built on data types owned by other N3O packages, which reach a site through uSync. That
    // import is manual outside development, so until it has been run there is nothing to build against and
    // the type is left for a later boot. That is an ordinary state, not a failure, so it is not logged as one
    private void Seed(string alias, Action seed) {
        if (Exists(alias)) {
            return;
        }

        try {
            seed();
        } catch (Exception ex) when (ex is DataTypeNotFoundException or ContentTypeNotFoundException) {
            _logger.LogInformation("Deferred platforms content type {Alias}: {Reason}", alias, ex.Message);
        } catch (ContentTypeKindMismatchException ex) {
            // Not something the site recovers from on its own, and not something to convert on its behalf,
            // so it is reported for someone to decide what the type should be
            _logger.LogWarning("Platforms content type {Alias} disagrees with the site: {Reason}",
                               alias,
                               ex.Message);
        } catch (Exception ex) {
            _logger.LogError(ex, "Could not create platforms content type {Alias}", alias);
        }
    }

    // Mirrors how the designer itself finds an existing type. An alias can be renamed while the key stays,
    // so matching on the alias alone would miss a type the designer then adopts by key and rewrites, and
    // would go on missing it every boot
    private bool Exists(string alias) {
        return _contentTypeService.Get(UmbracoId.Deterministic(IdScope.ContentType, alias)) != null ||
               _contentTypeService.Get(alias) != null;
    }
}
