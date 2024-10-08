﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class BooleanPropertyType : PropertyType<BooleanValueReq> {
    public BooleanPropertyType()
        : base("boolean",
               (ctx, src, dest) => dest.Boolean = ctx.Map<PublishedContentProperty, BooleanValueRes>(src),
               (ctx, src) => ctx.Map<ContentPropertyConfiguration, BooleanConfigurationRes>(src),
               UmbracoPropertyEditors.Aliases.Boolean) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                BooleanValueReq data) {
        contentBuilder.Boolean(alias).Set(data.Value.GetValueOrThrow());

        return Task.CompletedTask;
    }
}