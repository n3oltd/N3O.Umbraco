﻿using N3O.Umbraco.Content;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using UmbracoPropertyEditors = Umbraco.Cms.Core.Constants.PropertyEditors;

namespace N3O.Umbraco.Data.Lookups;

public class TextBoxPropertyType : PropertyType<TextBoxValueReq> {
    public TextBoxPropertyType()
        : base("textBox",
               (ctx, src, dest) => dest.TextBox = ctx.Map<PublishedContentProperty, TextBoxValueRes>(src),
               (ctx, src) => ctx.Map<ContentPropertyConfiguration, TextBoxConfigurationRes>(src),
               UmbracoPropertyEditors.Aliases.TextBox) { }

    protected override Task UpdatePropertyAsync(IContentBuilder contentBuilder,
                                                string alias,
                                                TextBoxValueReq data) {
        contentBuilder.TextBox(alias).Set(data.Value);

        return Task.CompletedTask;
    }
}