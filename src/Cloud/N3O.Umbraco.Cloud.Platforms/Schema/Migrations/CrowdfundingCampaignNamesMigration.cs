using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.ContentTypes;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Migrations;
using Aliases = N3O.Umbraco.Cloud.Platforms.PlatformsConstants.CrowdfundingCampaigns;

namespace N3O.Umbraco.Cloud.Platforms;

public class CrowdfundingCampaignNamesMigration : MigrationBase {
    private const string ItemName = "Crowdfunding Campaign";
    private const string ContainerName = "Crowdfunding Campaigns";

    // Both types were minted under their pre-rename aliases on every site, so a site's copy carries whichever
    // alias it was seeded under and no other key.
    private static readonly IReadOnlyList<Guid> ItemKeys = GetSeededKeys(Aliases.CrowdfundingCampaign.Alias,
                                                                          "platformsCrowdfunder");
    private static readonly IReadOnlyList<Guid> ContainerKeys = GetSeededKeys(Aliases.Alias, "platformsCrowdfunders");

    private readonly IContentTypeEditor _contentTypeEditor;
    private readonly IContentTypeService _contentTypeService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CrowdfundingCampaignNamesMigration(IMigrationContext context,
                                              IContentTypeEditor contentTypeEditor,
                                              IContentTypeService contentTypeService,
                                              IWebHostEnvironment webHostEnvironment)
        : base(context) {
        _contentTypeEditor = contentTypeEditor;
        _contentTypeService = contentTypeService;
        _webHostEnvironment = webHostEnvironment;
    }

    protected override void Migrate() {
        var item = FindSeeded(Aliases.CrowdfundingCampaign.Alias, ItemKeys);
        var container = FindSeeded(Aliases.Alias, ContainerKeys);

        SetName(item, ItemName);
        SetName(container, ContainerName);
    }

    // The alias may still be held by the legacy composition, which the seeder never touches and this step
    // must not either, so the holder has to be a type this package seeded and composed into nothing.
    private IContentType FindSeeded(string alias, IReadOnlyList<Guid> seededKeys) {
        var contentType = _contentTypeService.Get(alias);

        if (contentType == null) {
            return null;
        }

        var composedInto = _contentTypeService.GetComposedOf(contentType.Id).Select(x => x.Alias).ToList();

        if (!seededKeys.Contains(contentType.Key) || composedInto.Any()) {
            throw new Exception($"Content type {alias.Quote()} on site {Site.Id.Quote()} " +
                                $"({_webHostEnvironment.EnvironmentName}) is held under key {contentType.Key}" +
                                $"{(composedInto.Any() ? $" and composed into {string.Join(", ", composedInto)}" : "")}, " +
                                "so it is not the seeded type and was not renamed");
        }

        return contentType;
    }

    private void SetName(IContentType contentType, string name) {
        if (contentType == null || contentType.Name == name) {
            return;
        }

        var designer = _contentTypeEditor.ForExisting(contentType.Alias);

        designer.SetName(name, overwriteExisting: true);

        designer.Save();
    }

    private static IReadOnlyList<Guid> GetSeededKeys(params string[] aliases) {
        return aliases.Select(x => UmbracoId.Deterministic(IdScope.ContentType, x)).ToList();
    }
}
