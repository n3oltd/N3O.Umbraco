﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class NestedPropertyType : PropertyType<NestedValueReq> {
    public NestedPropertyType()
        : base("nested",
               (ctx, src, dest) => dest.Nested = ctx.Map<PublishedContentProperty, NestedValueRes>(src),
               (ctx, src) => ctx.Map<ContentPropertyConfiguration, NestedConfigurationRes>(src),
               UmbracoPropertyEditors.Aliases.NestedContent) { }

    protected override async Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                NestedValueReq data) {
        var nestedBuilder = contentBuilder.Nested(alias);

        foreach (var nestedValue in data.Items) {
            var builder = nestedBuilder.Add(nestedValue.ContentTypeAlias);

            foreach (var property in nestedValue.Properties) {
                await property.Type.UpdatePropertyAsync(builder, property.Alias, property.Value.Value);
            }
        }
    }
}