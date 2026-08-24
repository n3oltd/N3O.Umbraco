using Microsoft.Extensions.Logging;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud.Platforms;

public class CrowdfundersSchemaComponent : IComponent {
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

        RenameOntoServiceAliases();

        if (_contentTypeService.Value.Get(PlatformsConstants.CrowdfundingCampaigns.Alias) == null) {
            CreateDocumentTypes();
        }

        AllowCrowdfundersUnderPlatforms();
    }

    public void Terminate() { }

    // Only the campaign picker: the page properties differ per client and are added in the backoffice.
    private void CreateDocumentTypes() {
        var compositionAlias = PlatformsConstants.CrowdfundingCampaign.CompositionAlias;
        var legacyComposition = _contentTypeService.Value.Get(compositionAlias);

        if (legacyComposition == null) {
            _logger.LogInformation("No {Alias} composition found, skipping crowdfunder document types",
                                   compositionAlias);

            return;
        }

        CreateCrowdfunderDocumentType(legacyComposition.Icon);
        CreateCrowdfundersDocumentType();
    }

    // Checked every boot because a uSync import of the platforms type drops the container from its allowed children.
    private void AllowCrowdfundersUnderPlatforms() {
        if (_contentTypeService.Value.Get(PlatformsConstants.CrowdfundingCampaigns.Alias) == null) {
            return;
        }

        var platforms = _contentTypeService.Value.Get(PlatformsConstants.Platforms.Alias);

        if (platforms?.AllowedContentTypes.OrEmpty()
                      .Any(x => x.Alias == PlatformsConstants.CrowdfundingCampaigns.Alias) == true) {
            return;
        }

        var designer = (IDocumentTypeDesigner) _contentTypeEditor.Value
                                                                 .ForExisting(PlatformsConstants.Platforms.Alias);

        designer.AllowChildren(PlatformsConstants.CrowdfundingCampaigns.Alias);

        designer.Save();
    }

    private void CreateCrowdfunderDocumentType(string icon) {
        var alias = PlatformsConstants.CrowdfundingCampaigns.CrowdfundingCampaign.Alias;
        var designer = _contentTypeEditor.Value.NewDocument("Crowdfunder", alias);

        designer.SetIcon(icon);
        designer.InFolder("Platforms", "Crowdfunders");
        designer.WithDeterministicId();

        designer.Tab("Crowdfunder")
                .ContentPicker(PlatformsConstants.CrowdfundingCampaigns.CrowdfundingCampaign.Properties.Campaign)
                .Mandatory()
                .Description("The campaign this crowdfunder raises for");

        designer.Save();
    }

    private void RenameOntoServiceAliases() {
        // Old aliases are literals because the constants now hold the new values.
        RenameContentType("platformsCrowdfunder", PlatformsConstants.CrowdfundingCampaigns.CrowdfundingCampaign.Alias);
        RenameContentType("platformsCrowdfunders", PlatformsConstants.CrowdfundingCampaigns.Alias);
    }

    private void RenameContentType(string fromAlias, string toAlias) {
        var contentType = _contentTypeService.Value.Get(fromAlias);

        if (contentType == null) {
            return;
        }

        // Refuse rather than collide: uSync and Umbraco both silently mangle an alias clash.
        if (_contentTypeService.Value.Get(toAlias) != null) {
            _logger.LogError("Cannot rename {FromAlias} to {ToAlias} because {ToAlias} already exists - complete "
                             + "the crowdfunder migration on this site first", fromAlias, toAlias, toAlias);

            return;
        }

        contentType.Alias = toAlias;

        _contentTypeService.Value.Save(contentType);

        _logger.LogInformation("Renamed content type {FromAlias} to {ToAlias}", fromAlias, toAlias);
    }

    private void CreateCrowdfundersDocumentType() {
        var designer = _contentTypeEditor.Value.NewDocument("Crowdfunders",
                                                            PlatformsConstants.CrowdfundingCampaigns.Alias);

        designer.SetIcon("icon-file-cabinet color-black");
        designer.InFolder("Platforms", "Crowdfunders");
        designer.WithDeterministicId();
        designer.AllowChildren(PlatformsConstants.CrowdfundingCampaigns.CrowdfundingCampaign.Alias);

        designer.Save();
    }
}
