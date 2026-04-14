using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class HoldQurbaniCartExtensionExtensions {
    public static IReadOnlyDictionary<string, object> GetExtensionData(this IQurbaniCartExtensionContent umbracoContent,
                                                                       Action<QurbaniCartExtensionsReq> populate = null) {
        var extensionData = new QurbaniCartExtensionsReq();

        extensionData.Season = umbracoContent.Content()
                                             .Ancestors()
                                             .Single(x => x.ContentType.Alias.EqualsInvariant(PlatformsConstants.Qurbani.Settings.Season.Alias))
                                             .Name;

        populate?.Invoke(extensionData);

        return new Dictionary<string, object> {
            { QurbaniCartExtensionKey.Qurbani.ToEnumString(), extensionData }
        };
    }
}
