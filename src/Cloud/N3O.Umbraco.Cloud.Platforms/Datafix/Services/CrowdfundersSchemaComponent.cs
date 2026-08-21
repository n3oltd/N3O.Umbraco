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

        if (_contentTypeService.Value.Get(PlatformsConstants.Crowdfunders.Alias) == null) {
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

    private void CreateCrowdfunderDocumentType(string icon) {
        var designer = _contentTypeEditor.Value.NewDocument("Crowdfunder",
                                                            PlatformsConstants.Crowdfunders.Crowdfunder.Alias);

        designer.SetIcon(icon);
        designer.InFolder("Platforms", "Crowdfunders");
        designer.WithDeterministicId();

        designer.Tab("Crowdfunder")
                .ContentPicker(PlatformsConstants.Crowdfunders.Crowdfunder.Properties.Campaign)
                .Mandatory()
                .Description("The campaign this crowdfunder raises for");

        designer.Save();
    }

    // TODO Wave 3 - once every site has completed wave 2, uncomment RenameOntoServiceAliases, call it first from
    // Initialize, and in the SAME release flip these constants and rename their classes to match:
    //     Crowdfunders.Alias              "platformsCrowdfunders"  ->  "platformsCrowdfundingCampaigns"
    //     Crowdfunders.Crowdfunder.Alias  "platformsCrowdfunder"   ->  "platformsCrowdfundingCampaign"
    // The stored alias and the compiled constant have to move together or the content model stops binding, so this
    // cannot be an endpoint. Regenerate the site's uSync ContentTypes configs and models in the same change.
    //
    // private void RenameOntoServiceAliases() {
    //     // Old aliases are literals because the constants now hold the new values.
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
    //     // Refuse rather than collide: uSync and Umbraco both silently mangle an alias clash.
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
