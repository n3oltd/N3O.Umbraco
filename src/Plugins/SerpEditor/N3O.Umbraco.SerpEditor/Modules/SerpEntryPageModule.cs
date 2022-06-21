using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using N3O.Umbraco.SerpEditor.Content;
using N3O.Umbraco.SerpEditor.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.SerpEditor.Modules;

public class SerpEntryPageModule : IPageModule {
    private readonly Lazy<IContentCache> _contentCache;

    public SerpEntryPageModule(Lazy<IContentCache> contentCache) {
        _contentCache = contentCache;
    }

    public bool ShouldExecute(IPublishedContent page) => true;

    public Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        SerpEntry serpEntry = null;
        
        foreach (var property in page.Properties) {
            if (property.PropertyType.EditorAlias.EqualsInvariant(SerpEditorConstants.PropertyEditorAlias)) {
                serpEntry = new SerpEntry((SerpEntry) property.GetValue());

                break;
            }
        }

        if (serpEntry == null) {
            serpEntry = new SerpEntry();
        } else {
            var template = _contentCache.Value.Single<TemplateContent>();

            if (template.HasValue(x => x.TitleSuffix)) {
                serpEntry.Title += $" {template.TitleSuffix}";
            }
        }

        return Task.FromResult<object>(serpEntry);
    }

    public string Key => SerpEditorConstants.PageModuleKeys.SerpEntry;
}
