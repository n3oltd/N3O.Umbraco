using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Strings;
using Umbraco.Cms.Infrastructure.Migrations;
using Umbraco.Extensions;
using Aliases = N3O.Umbraco.Cloud.Platforms.PlatformsConstants.CrowdfundingCampaigns;
using Descriptions = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Descriptions;
using Folders = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Folders;
using Names = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Names;
using Tabs = N3O.Umbraco.Cloud.Platforms.PlatformsSchemaConstants.Tabs;

namespace N3O.Umbraco.Cloud.Platforms;

public class CrowdfundingCampaignNamesMigration : MigrationBase {
    private const string LegacyContainerAlias = "platformsCrowdfunders";
    private const string LegacyFolder = "Crowdfunders";
    private const string LegacyItemAlias = "platformsCrowdfunder";

    private static readonly IReadOnlyList<Guid> ContainerKeys = GetSeededKeys(Aliases.Alias, LegacyContainerAlias);
    private static readonly IReadOnlyList<Guid> ItemKeys = GetSeededKeys(Aliases.CrowdfundingCampaign.Alias,
                                                                          LegacyItemAlias);
    private static readonly string[] LegacyCrowdfunderPageTemplateTabs = [
        "newCrowdfunderTemplate",
        "fundraisingPageTemplate"
    ];
    private static readonly string[] LegacyCrowdfundingCampaignTabs = ["Crowdfunder"];

    private readonly IContentTypeService _contentTypeService;
    private readonly IShortStringHelper _shortStringHelper;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CrowdfundingCampaignNamesMigration(IMigrationContext context,
                                              IContentTypeService contentTypeService,
                                              IShortStringHelper shortStringHelper,
                                              IWebHostEnvironment webHostEnvironment)
        : base(context) {
        _contentTypeService = contentTypeService;
        _shortStringHelper = shortStringHelper;
        _webHostEnvironment = webHostEnvironment;
    }

    protected override void Migrate() {
        var item = FindSeeded(Aliases.CrowdfundingCampaign.Alias, ItemKeys);
        var container = FindSeeded(Aliases.Alias, ContainerKeys);

        if (item != null) {
            RenameTab(item, LegacyCrowdfundingCampaignTabs, Tabs.CrowdfundingCampaign);
            RenameTab(item, LegacyCrowdfunderPageTemplateTabs, Tabs.CrowdfunderPageTemplate);
            SetCampaignDescription(item);
            item.Name = Names.CrowdfundingCampaign;

            _contentTypeService.Save(item);
        }

        if (container != null) {
            container.Name = Names.CrowdfundingCampaigns;

            _contentTypeService.Save(container);
        }

        RenameFolder();
    }

    private IContentType FindSeeded(string alias, IReadOnlyList<Guid> seededKeys) {
        var contentType = _contentTypeService.Get(alias);

        if (contentType == null) {
            return null;
        }

        var composedInto = _contentTypeService.GetComposedOf(contentType.Id).Select(x => x.Alias).ToList();

        if (!seededKeys.Contains(contentType.Key) || composedInto.Any()) {
            var composition = composedInto.Any() ? $" and composed into {string.Join(", ", composedInto)}" : "";

            throw new Exception($"Content type {alias.Quote()} on site {Site.Id.Quote()} " +
                                $"({_webHostEnvironment.EnvironmentName}) is held under key {contentType.Key}" +
                                $"{composition}, so it is not the seeded type and was not renamed");
        }

        return contentType;
    }

    private void RenameTab(IContentType contentType, IReadOnlyList<string> legacyAliases, string name) {
        var alias = name.ToSafeAlias(_shortStringHelper, true);

        if (contentType.PropertyGroups.Any(x => x.Alias.EqualsInvariant(alias))) {
            return;
        }

        var tab = contentType.PropertyGroups
                             .FirstOrDefault(x => x.Type == PropertyGroupType.Tab &&
                                                  legacyAliases.Contains(x.Alias, true));

        if (tab == null) {
            return;
        }

        var legacyAlias = tab.Alias;

        tab.Alias = alias;
        tab.Name = name;

        foreach (var group in contentType.PropertyGroups) {
            if (group.Alias.StartsWith($"{legacyAlias}/", StringComparison.InvariantCultureIgnoreCase)) {
                group.Alias = $"{alias}{group.Alias.Substring(legacyAlias.Length)}";
            }
        }
    }

    private void SetCampaignDescription(IContentType contentType) {
        var campaign = contentType.PropertyTypes
                                  .FirstOrDefault(x => x.Alias == Aliases.CrowdfundingCampaign.Properties.Campaign);

        if (campaign != null) {
            campaign.Description = Descriptions.CrowdfundingCampaignCampaign;
        }
    }

    private void RenameFolder() {
        var platforms = _contentTypeService.GetContainers(Folders.Platforms, 1).SingleOrDefault();

        if (platforms == null) {
            return;
        }

        var folders = _contentTypeService.GetContainers(Folders.CrowdfundingCampaigns, platforms.Level + 1)
                                         .Concat(_contentTypeService.GetContainers(LegacyFolder, platforms.Level + 1))
                                         .Where(x => x.ParentId == platforms.Id)
                                         .ToList();

        var folder = folders.SingleOrDefault(x => x.Name == LegacyFolder);

        if (folder == null || folders.Any(x => x.Name == Folders.CrowdfundingCampaigns)) {
            return;
        }

        folder.Name = Folders.CrowdfundingCampaigns;

        var attempt = _contentTypeService.SaveContainer(folder);

        if (!attempt.Success) {
            throw attempt.Exception ?? new Exception($"Could not rename the {LegacyFolder.Quote()} content type folder " +
                                                     $"({attempt.Result?.Result})");
        }
    }

    private static IReadOnlyList<Guid> GetSeededKeys(params string[] aliases) {
        return aliases.Select(x => UmbracoId.Deterministic(IdScope.ContentType, x)).ToList();
    }
}
