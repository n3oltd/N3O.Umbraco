using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.PublishedCache;

namespace N3O.Umbraco.EditorJs;

public class ListBlockDataConverter : BlockDataConverter<ListBlockData> {
    public ListBlockDataConverter(IPublishedContentCache contentCache, IPublishedMediaCache mediaCache,
                                  IPublishedUrlProvider publishedUrlProvider)
        : base(contentCache, mediaCache, publishedUrlProvider) { }

    protected override string TypeId => "list";

    protected override void Process(ListBlockData data) {
        if (data.Items.HasAny()) {
            data.Items = data.Items.Select(ConvertUmbracoLinks).ToList();
        }
    }
}

public class ListBlockData {
    public string Style { get; set; }
    public IEnumerable<string> Items { get; set; }
}