﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.AccountEntry.Address)]
public class AddressEntryContent : UmbracoContent<AddressEntryContent> {
    public virtual string GoogleMapsApiKey => GetValue(x => x.GoogleMapsApiKey);
}