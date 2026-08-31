using Microsoft.Extensions.Logging;
using N3O.Umbraco.ContentTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms;

public partial class PlatformsContentTypeSeeder : IPlatformsContentTypeSeeder {
    private readonly IContentTypeEditor _contentTypeEditor;
    private readonly IDataTypeService _dataTypeService;
    private readonly ILogger<PlatformsContentTypeSeeder> _logger;

    public PlatformsContentTypeSeeder(IContentTypeEditor contentTypeEditor,
                                      IDataTypeService dataTypeService,
                                      ILogger<PlatformsContentTypeSeeder> logger) {
        _contentTypeEditor = contentTypeEditor;
        _dataTypeService = dataTypeService;
        _logger = logger;
    }

    public IReadOnlyList<string> Aliases => GetSeeds().Select(x => x.Alias).ToList();

    public void Seed() {
        foreach (var seed in GetSeeds()) {
            Seed(seed.Alias, seed.Run);
        }
    }

    // Elements come first because the document types compose them, and a composition resolves by alias when
    // the type that composes it saves. For the same reason a type is seeded before any type that names it as
    // an allowed child, which is why a season category precedes its season and the calculator runs leaf
    // upward: seeded the other way round, each boot completes one link and the tree takes as many restarts
    private IReadOnlyList<(string Alias, Action Run)> GetSeeds() {
        return [
            (PlatformsConstants.DonationFormState.SuggestedAmount, SeedSuggestedAmount),
            (PlatformsConstants.DonationFormContent.CompositionAlias, SeedDonationFormContent),
            (PlatformsConstants.DonationFormState.CompositionAlias, SeedDonationFormState),
            (PlatformsConstants.DonationFormState.Feedback, SeedFeedbackDonationFormState),
            (PlatformsConstants.DonationFormState.Fund, SeedFundDonationFormState),
            (PlatformsConstants.DonationFormState.Qurbani, SeedQurbaniDonationFormState),
            (PlatformsConstants.DonationFormState.Sponsorship, SeedSponsorshipDonationFormState),

            (PlatformsConstants.Platforms.Alias, SeedPlatforms),

            (PlatformsConstants.Campaigns.CompositionAlias, SeedCampaign),
            (PlatformsConstants.Campaigns.Standard, SeedStandardCampaign),
            (PlatformsConstants.Campaigns.Giving, SeedGivingCampaign),
            (PlatformsConstants.Campaigns.Qurbani, SeedQurbaniCampaign),
            (PlatformsConstants.Campaigns.Telethon, SeedTelethonCampaign),
            (PlatformsConstants.Campaigns.ScheduledGiving, SeedScheduledGivingCampaign),
            (PlatformsConstants.Campaigns.RegularGiving, SeedRegularGivingCampaign),
            (PlatformsConstants.Offerings.CompositionAlias, SeedOffering),
            (PlatformsConstants.CrossSells.CompositionAlias, SeedCrossSell),
            (PlatformsConstants.Qurbani.Season.Category.Alias, SeedQurbaniSeasonCategory),
            (PlatformsConstants.Qurbani.Season.Alias, SeedQurbaniSeason),

            (PlatformsConstants.Zakat.Settings.Calculator.Field.Alias, SeedZakatCalculatorField),
            (PlatformsConstants.Zakat.Settings.Calculator.Section.Alias, SeedZakatCalculatorSection),
            (PlatformsConstants.Zakat.Settings.Calculator.Alias, SeedZakatCalculatorSettings)
        ];
    }

    // A subscription defines however many fund dimensions it uses, so the data type behind a higher
    // dimension legitimately does not exist on every site and that property is simply not part of its schema
    private bool HasDataType(string name) {
        return _dataTypeService.GetDataType(name) != null;
    }

    // Only a type the site does not have is created. A type it already holds is left exactly as its editors
    // have shaped it, and is changed only by a migration step, so a property a site has deliberately removed
    // is never put back.
    // Every type is built on data types owned by other N3O packages, which reach a site through uSync. That
    // import is manual outside development, so until it has been run there is nothing to build against and
    // the type is left for a later boot. That is an ordinary state, not a failure, so it is not logged as one
    private void Seed(string alias, Action seed) {
        if (_contentTypeEditor.Find(alias) != null) {
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
}
