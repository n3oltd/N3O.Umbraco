﻿using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Pages;
using N3O.Umbraco.Templates.Extensions;
using System.Threading.Tasks;

namespace N3O.Umbraco.Templates.TagHelpers;

[HtmlTargetElement("n3o-merged-content")]
public class MergedContentTagHelper : TagHelper {
    private readonly ITemplateEngine _templateEngine;

    public MergedContentTagHelper(ITemplateEngine templateEngine) {
        _templateEngine = templateEngine;
    }
    
    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var tagHelperContent = output.IsContentModified ? output.Content : await output.GetChildContentAsync();
        var htmlContent = new StringHtmlContent(tagHelperContent.GetContent());
        var mergeModels = Model.MergeModels();
        var mergedHtmlContent = new MergedHtmlContent(_templateEngine, htmlContent, mergeModels);

        output.TagName = null;
        output.Content.SetHtmlContent(mergedHtmlContent);
    }
}