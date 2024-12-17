using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using N3O.Umbraco.PageTitle;
using N3O.Umbraco.SerpEditor.Content;
using N3O.Umbraco.SerpEditor.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.SerpEditor.Modules;

public class SerpEntryPageModule : IPageModule {
    private static readonly ConcurrentDictionary<string, string> PagePropertyAliasCache = new();
    
    private readonly Lazy<IContentCache> _contentCache;
    private readonly IEnumerable<IPageTitleProvider> _pageTitleProviders;

    public SerpEntryPageModule(Lazy<IContentCache> contentCache, IEnumerable<IPageTitleProvider> pageTitleProviders) {
        _contentCache = contentCache;
        _pageTitleProviders = pageTitleProviders;
    }

    public bool ShouldExecute(IPublishedContent page) => true;

    public Task<object> ExecuteAsync(IPublishedContent page, CancellationToken cancellationToken) {
        SerpEntry serpEntry = null;
        
        var propertyAlias = PagePropertyAliasCache.GetOrAdd(page.ContentType.Alias, _ => {
            foreach (var property in page.Properties) {
                if (property.PropertyType.EditorAlias.EqualsInvariant(SerpEditorConstants.PropertyEditorAlias)) {
                    return property.Alias;
                }
            }

            return null;
        });
        
        if (propertyAlias.HasValue()) {
            serpEntry = new SerpEntry((SerpEntry) page.GetProperty(propertyAlias)?.GetValue());
        }
        
        var pageTitleProvider = _pageTitleProviders.OrEmpty().FirstOrDefault(x => x.IsProviderFor(page));

        if (pageTitleProvider.HasValue()) {
            serpEntry = new SerpEntry {
                Title = pageTitleProvider.GetPageTitle(page),
                Description = serpEntry?.Description
            };
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
