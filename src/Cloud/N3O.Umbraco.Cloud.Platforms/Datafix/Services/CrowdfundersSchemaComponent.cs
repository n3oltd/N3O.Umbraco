using Microsoft.Extensions.Logging;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms;

public class CrowdfundersSchemaComponent : IComponent {
    private const string PageTab = "Page";
    private const string PageTemplateTab = "New Crowdfunder Page Template";

    private readonly Lazy<IContentTypeEditor> _contentTypeEditor;
    private readonly Lazy<IContentTypeService> _contentTypeService;
    private readonly ILogger<CrowdfundersSchemaComponent> _logger;
    private readonly IRuntimeState _runtimeState;

    public CrowdfundersSchemaComponent(Lazy<IContentTypeEditor> contentTypeEditor,
                                       Lazy<IContentTypeService> contentTypeService,
                                       ILogger<CrowdfundersSchemaComponent> logger,
                                       IRuntimeState runtimeState) {
        _contentTypeEditor = contentTypeEditor;
        _contentTypeService = contentTypeService;
        _logger = logger;
        _runtimeState = runtimeState;
    }

    public void Initialize() {
        if (_runtimeState.Level != RuntimeLevel.Run) {
            return;
        }

        if (_contentTypeService.Value.Get(PlatformsConstants.Crowdfunders.Alias) == null) {
            CreateDocumentTypes();
        }

        AllowCrowdfundersUnderPlatforms();
    }

    public void Terminate() { }

    private void CreateDocumentTypes() {
        var compositionAlias = PlatformsConstants.CrowdfundingCampaign.CompositionAlias;
        var legacyComposition = _contentTypeService.Value.Get(compositionAlias);

        if (legacyComposition == null) {
            _logger.LogInformation("No {Alias} composition found, skipping crowdfunder document types",
                                   compositionAlias);

            return;
        }

        // A raw copy is only valid when source and destination share a data type, so every new property reuses
        // the DataTypeKey of the legacy property it is populated from rather than a hardcoded key.
        var legacyProperties = legacyComposition.PropertyTypes.ToList();
        var contentAlias = PlatformsConstants.CrowdfundingCampaign.Properties.Content;
        var heroImageAlias = PlatformsConstants.CrowdfundingCampaign.Properties.HeroImage;

        var contentDataTypeKey = legacyProperties.Single(x => x.Alias == contentAlias).DataTypeKey;

        var heroImageDataTypeKey = legacyProperties.SingleOrDefault(x => x.Alias == heroImageAlias)?.DataTypeKey;

        if (!heroImageDataTypeKey.HasValue()) {
            FindCampaignProperty(PlatformsConstants.Campaigns.Properties.HeroImage, out var campaignHeroImage);

            heroImageDataTypeKey = campaignHeroImage?.DataTypeKey;
        }

        CreateCrowdfunderDocumentType(legacyComposition.Icon, contentDataTypeKey, heroImageDataTypeKey);
        CreateCrowdfundersDocumentType();
    }

    // Checked on every boot because a uSync import of the platforms type drops the container from its allowed
    // children and nothing else restores it.
    private void AllowCrowdfundersUnderPlatforms() {
        if (_contentTypeService.Value.Get(PlatformsConstants.Crowdfunders.Alias) == null) {
            return;
        }

        var platforms = _contentTypeService.Value.Get(PlatformsConstants.Platforms.Alias);

        if (platforms?.AllowedContentTypes.OrEmpty()
                      .Any(x => x.Alias == PlatformsConstants.Crowdfunders.Alias) == true) {
            return;
        }

        var designer = (IDocumentTypeDesigner) _contentTypeEditor.Value
                                                                 .ForExisting(PlatformsConstants.Platforms.Alias);

        designer.AllowChildren(PlatformsConstants.Crowdfunders.Alias);

        designer.Save();
    }

    private void CreateCrowdfunderDocumentType(string icon, Guid contentDataTypeKey, Guid? heroImageDataTypeKey) {
        var designer = _contentTypeEditor.Value.NewDocument("Crowdfunder",
                                                            PlatformsConstants.Crowdfunders.Crowdfunder.Alias);

        designer.SetIcon(icon);
        designer.InFolder("Platforms", "Crowdfunders");
        designer.WithDeterministicId();

        var page = designer.Tab(PageTab);
        var template = designer.Tab(PageTemplateTab);
        var carriedOver = ResolveCarriedOverProperties();

        page.ContentPicker(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.Campaign)
            .Mandatory()
            .Description("The campaign this crowdfunder raises for");

        if (heroImageDataTypeKey.HasValue()) {
            page.ExistingDataType(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.PageHeroImage)
                .DataType(heroImageDataTypeKey.GetValueOrThrow().ToString())
                .Description("Hero image for the campaign's own crowdfunding page");

            template.ExistingDataType(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.PageTemplateHeroImage)
                    .DataType(heroImageDataTypeKey.GetValueOrThrow().ToString())
                    .Description("Hero image each new fundraising page is created with");
        }

        AddCarriedOverProperties(page,
                                 template,
                                 carriedOver.FirstOrDefault(x => x.Key.Name.EqualsInvariant(PageTab)).Value);

        page.ExistingDataType(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.Page)
            .DataType(contentDataTypeKey.ToString())
            .Description("Content for the campaign's own crowdfunding page");

        template.ExistingDataType(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.PageTemplate)
                .DataType(contentDataTypeKey.ToString())
                .Description("Content each new fundraising page is created from");

        foreach (var (group, properties) in carriedOver.Where(x => !x.Key.Name.EqualsInvariant(PageTab))
                                                      .OrderBy(x => x.Key.SortOrder)) {
            AddCarriedOverProperties(page.Group(group.Name), template.Group(group.Name), properties);
        }

        designer.Save();
    }

    private void AddCarriedOverProperties(IPropertyContainerBuilder page,
                                          IPropertyContainerBuilder template,
                                          List<IPropertyType> properties) {
        foreach (var property in properties.OrEmpty().OrderBy(x => x.SortOrder)) {
            page.ExistingDataType(CrowdfunderContentSources.PageAlias(property.Alias))
                .DataType(property.DataTypeKey.ToString())
                .Name(property.Name)
                .Description(property.Description);

            template.ExistingDataType(CrowdfunderContentSources.PageTemplateAlias(property.Alias))
                    .DataType(property.DataTypeKey.ToString())
                    .Name(property.Name)
                    .Description(property.Description);
        }
    }

    private Dictionary<PropertyGroup, List<IPropertyType>> ResolveCarriedOverProperties() {
        var byGroup = new Dictionary<PropertyGroup, List<IPropertyType>>();

        foreach (var alias in CrowdfunderContentSources.CarriedOver) {
            var group = FindCampaignProperty(alias, out var property);

            if (group == null) {
                _logger.LogWarning("No campaign property {Alias} found, so it is not added to the crowdfunder",
                                   alias);

                continue;
            }

            var existing = byGroup.Keys.FirstOrDefault(x => x.Name.EqualsInvariant(group.Name));

            if (existing == null) {
                byGroup[group] = [property];
            } else {
                byGroup[existing].Add(property);
            }
        }

        return byGroup;
    }

    private PropertyGroup FindCampaignProperty(string alias, out IPropertyType property) {
        property = null;

        var campaignComposition = _contentTypeService.Value.Get(PlatformsConstants.Campaigns.CompositionAlias);

        if (campaignComposition == null) {
            return null;
        }

        foreach (var campaignType in _contentTypeService.Value.GetComposedOf(campaignComposition.Id)) {
            foreach (var group in campaignType.CompositionPropertyGroups.OrEmpty()) {
                var match = group.PropertyTypes.OrEmpty().FirstOrDefault(x => x.Alias == alias);

                if (match != null) {
                    property = match;

                    return group;
                }
            }
        }

        return null;
    }

    // TODO Wave 3 - rename the crowdfunder types onto the aliases the crowdfunding service uses, once every site
    // has completed wave 2 (the dev tools crowdfunders/migration/complete endpoint, which deletes the legacy
    // composition and so frees the alias). Uncomment RenameOntoServiceAliases below, call it from Initialize
    // before the other steps, and in the SAME release change these constants, because the stored alias and the
    // compiled constant have to move together or the content model silently stops binding:
    //     Crowdfunders.Alias                  "platformsCrowdfunders"  ->  "platformsCrowdfundingCampaigns"
    //     Crowdfunders.Crowdfunder.Alias      "platformsCrowdfunder"   ->  "platformsCrowdfundingCampaign"
    // and rename the constant classes to match (Crowdfunders -> CrowdfundingCampaigns, Crowdfunder ->
    // CrowdfundingCampaign), which is only free once the legacy CrowdfundingCampaign constants are deleted.
    // A site must be through wave 2 before it takes that release: the rename refuses while the legacy type still
    // holds the alias, and until it runs that site's crowdfunder publishing is broken. This cannot be an endpoint
    // for the same reason - the constant flip arrives with the deployment, so the rename has to as well.
    // The site's uSync ContentTypes configs and generated models have to be regenerated in the same change, or a
    // later uSync import matches the stale composition config by alias and overwrites the renamed document type.
    //
    // private void RenameOntoServiceAliases() {
    //     // The old aliases are literals because the constants now hold the new values. Deleted with this method.
    //     RenameContentType("platformsCrowdfunder", PlatformsConstants.Crowdfunders.Crowdfunder.Alias);
    //     RenameContentType("platformsCrowdfunders", PlatformsConstants.Crowdfunders.Alias);
    // }
    //
    // private void RenameContentType(string fromAlias, string toAlias) {
    //     var contentType = _contentTypeService.Value.Get(fromAlias);
    //
    //     if (contentType == null) {
    //         return;
    //     }
    //
    //     // Refuse rather than collide: uSync and Umbraco both resolve an alias clash by mangling it, so renaming
    //     // onto an alias something else still holds corrupts both types silently.
    //     if (_contentTypeService.Value.Get(toAlias) != null) {
    //         _logger.LogError("Cannot rename {FromAlias} to {ToAlias} because {ToAlias} already exists - complete "
    //                          + "the crowdfunder migration on this site first", fromAlias, toAlias, toAlias);
    //
    //         return;
    //     }
    //
    //     contentType.Alias = toAlias;
    //
    //     _contentTypeService.Value.Save(contentType);
    //
    //     _logger.LogInformation("Renamed content type {FromAlias} to {ToAlias}", fromAlias, toAlias);
    // }

    private void CreateCrowdfundersDocumentType() {
        var designer = _contentTypeEditor.Value.NewDocument("Crowdfunders", PlatformsConstants.Crowdfunders.Alias);

        designer.SetIcon("icon-file-cabinet color-black");
        designer.InFolder("Platforms", "Crowdfunders");
        designer.WithDeterministicId();
        designer.AllowChildren(PlatformsConstants.Crowdfunders.Crowdfunder.Alias);

        designer.Save();
    }
}
