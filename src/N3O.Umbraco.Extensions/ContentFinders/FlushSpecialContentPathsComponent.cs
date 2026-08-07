using N3O.Umbraco.Content;
using System;
using Umbraco.Cms.Core.Composing;

namespace N3O.Umbraco.ContentFinders;

public class FlushSpecialContentPathsComponent : IComponent {
    private readonly IContentCache _contentCache;

    public FlushSpecialContentPathsComponent(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public void Initialize() {
        _contentCache.Flushed += ContentCacheOnFlushed;
    }

    public void Terminate() {
        _contentCache.Flushed -= ContentCacheOnFlushed;
    }

    private void ContentCacheOnFlushed(object sender, EventArgs args) {
        SpecialContentPathParser.Flush();
    }
}
