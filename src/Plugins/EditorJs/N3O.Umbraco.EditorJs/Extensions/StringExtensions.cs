using AngleSharp;
using AngleSharp.Dom;
using N3O.Umbraco.EditorJs.Models;
using N3O.Umbraco.Markup;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.EditorJs.Extensions;

public static class StringExtensions {
    public static EditorJsModel MarkdownToEditorJsModel(this string markdown, IMarkupEngine markupEngine) {
        var model = new EditorJsModel();

        model.Blocks = MarkdownToEditorJsBlockBases(markupEngine, markdown);
        model.Version = "1";
        model.LastModified = DateTime.UtcNow.Ticks;
        
        return model;
    }

    private static IEnumerable<EditorJsBlock> MarkdownToEditorJsBlockBases(IMarkupEngine markupEngine,
                                                                           string markdown) {
        var html = markupEngine.RenderHtml(markdown);
        var blocks = GetBlocks(html.Value);

        return blocks;
    }
    
    public static IEnumerable<EditorJsBlock> GetBlocks(string html) {
        var context = BrowsingContext.New();
        var document = context.OpenAsync(req => req.Content(html)).GetAwaiter().GetResult();

        foreach (var element in document.Body.Children) {
            if (element.TagName == "p") {
                var paragraphBlockData = new ParagraphBlockData();
                paragraphBlockData.Text = element.InnerHtml;
                
                yield return new EditorJsBlock<ParagraphBlockData>("{id}", "paragraph", paragraphBlockData);
            } else if (element.TagName == "ul") {
                var items = new List<string>();
                
                foreach (var liElement in element.Children) {
                    items.Add(liElement.InnerHtml);
                }
                
                var listBlockData = new ListBlockData();
                listBlockData.Items = items;
                
                yield return new EditorJsBlock<ListBlockData>("{id}", "list", listBlockData);
            } else if (element.TagName == "h1") {
                var headerBlockData = new HeaderBlockData();
                headerBlockData.Level = 1;
                headerBlockData.Text = element.Text();
                
                yield return new EditorJsBlock<HeaderBlockData>("{id}", "header", headerBlockData);
            } else if (element.TagName == "h2") {
                var headerBlockData = new HeaderBlockData();
                headerBlockData.Level = 2;
                headerBlockData.Text = element.Text();
                
                yield return new EditorJsBlock<HeaderBlockData>("{id}", "header", headerBlockData);
            } else if (element.TagName == "h3") {
                var headerBlockData = new HeaderBlockData();
                headerBlockData.Level = 3;
                headerBlockData.Text = element.Text();
                
                yield return new EditorJsBlock<HeaderBlockData>("{id}", "header", headerBlockData);
            } else if (element.TagName == "h4") {
                var headerBlockData = new HeaderBlockData();
                headerBlockData.Level = 4;
                headerBlockData.Text = element.Text();
                
                yield return new EditorJsBlock<HeaderBlockData>("{id}", "header", headerBlockData);
            }
        }
    }
}