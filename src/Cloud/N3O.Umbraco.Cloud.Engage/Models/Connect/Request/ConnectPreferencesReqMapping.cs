﻿using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Cloud.Engage.Clients;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Engage.Models;

public class ConnectPreferencesReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IConsent, ConnectPreferencesReq>((_, _) => new ConnectPreferencesReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IConsent src, ConnectPreferencesReq dest, MapperContext ctx) {
        dest.PrivacyStatement = "web";
        dest.Selections = src.Choices
                             .OrEmpty()
                             .Select(ctx.Map<IConsentChoice, ConnectPreferenceSelectionReq>)
                             .ToList();
    }
}