using Microsoft.Extensions.Logging;
using N3O.Umbraco.Blocks;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Notifications;

public class ZakatCalculatorSettingsPublished : CloudContentPublished {
    private readonly Lazy<IContentLocator> _contentLocator;
    private readonly IUmbracoMapper _mapper;
    private readonly IBlocksRenderer _blocksRenderer;

    public ZakatCalculatorSettingsPublished(ISubscriptionAccessor subscriptionAccessor,
                                            ICloudUrl cloudUrl,
                                            IBackgroundJob backgroundJob,
                                            Lazy<IContentLocator> contentLocator,
                                            IUmbracoMapper mapper,
                                            ILogger<CloudContentPublished> logger,
                                            IBlocksRenderer blocksRenderer)
        : base(subscriptionAccessor, cloudUrl, backgroundJob, logger) {
        _contentLocator = contentLocator;
        _mapper = mapper;
        _blocksRenderer = blocksRenderer;
    }

    protected override async Task<object> GetBodyAsync(IContent content) {
        var settingsContent = _contentLocator.Value.ById<ZakatCalculatorSettingsContent>(content.Key);

        var settingsReq = new ZakatPlatformsSettingsReq();
        settingsReq.Calculator = new ZakatCalculatorSettingsReq();
        settingsReq.Calculator.DonationFormState = _mapper.Map<OfferingContent, DonationFormStateReq>(settingsContent.Offering);
        settingsReq.Calculator.DonationFormState.Options = new DonationFormOptionsReq();
        settingsReq.Calculator.DonationFormState.Options.SuggestedFilters = new DonationFormSuggestedFiltersReq();
        settingsReq.Calculator.DonationFormState.Options.SuggestedFilters.FundDimensions = new FundDimensionsFilterReq();
        settingsReq.Calculator.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension1 = settingsContent.FundDimension1?.Name;
        settingsReq.Calculator.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension2 = settingsContent.FundDimension2?.Name;
        settingsReq.Calculator.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension3 = settingsContent.FundDimension3?.Name;
        settingsReq.Calculator.DonationFormState.Options.SuggestedFilters.FundDimensions.Dimension4 = settingsContent.FundDimension4?.Name;
        
        settingsReq.Calculator.Sections = (await settingsContent.Sections.Where(x => x.Fields.HasAny()).SelectListAsync(GetSectionsAsync)).ToList();
        
        return settingsReq;
    }

    private async Task<ZakatCalculatorSectionReq> GetSectionsAsync(ZakatCalculatorSectionSettingsContent section) {
        var req = new ZakatCalculatorSectionReq();
        
        req.Alias = section.Alias;
        req.Name = section.Name;
        req.Fields = (await section.Fields.SelectListAsync(GetFieldsAsync)).ToList();

        if (section.Content.HasValue()) {
            req.Content = new RichTextContentReq();
            req.Content.Html = (await _blocksRenderer.RenderBlocksAsync(section.Content(),
                                                                        AliasHelper<ZakatCalculatorSectionSettingsContent>.PropertyAlias(x => x.Content)))
               .ToHtmlString();
        }
        
        return req;
    }

    private async Task<ZakatCalculatorFieldReq> GetFieldsAsync(ZakatCalculatorFieldSettingsContent field) {
        var req = new ZakatCalculatorFieldReq();
        
        req.Classification = field.Classification.ToEnum<CalculatorFieldClassification>();
        req.Type = field.Type.ToEnum<CalculatorFieldType>();
        req.Alias = field.Alias;
        req.Name = field.Name;
        req.Tooltip = field.Tooltip;
        
        if (field.Content.HasValue()) {
            req.Content = new RichTextContentReq();
            req.Content.Html = (await _blocksRenderer.RenderBlocksAsync(field.Content(),
                                                                        AliasHelper<ZakatCalculatorFieldSettingsContent>.PropertyAlias(x => x.Content)))
               .ToHtmlString();
        }

        if (field.Metal.HasValue()) {
            req.Metal = new ZakatCalculatorMetalFieldReq();
            req.Metal.Metal = field.Metal.ToEnum<Clients.Metal>();
        }
        
        return req;
    }


    protected override bool CanProcess(IContent content) {
        return content.IsZakatCalculatorSettings() ||
               content.IsZakatCalculatorSection() ||
               content.IsZakatCalculatorField();
    }

    protected override string HookId => PlatformsConstants.WebhookIds.ZakatSettings;
}