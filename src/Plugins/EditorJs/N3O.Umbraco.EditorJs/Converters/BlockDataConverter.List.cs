using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class ListBlockDataConverter : BlockDataConverter<ListBlockData, None> {
    public ListBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                  IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }

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