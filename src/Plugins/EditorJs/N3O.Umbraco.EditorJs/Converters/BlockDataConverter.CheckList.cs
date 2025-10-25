using System.Collections.Generic;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.EditorJs;

public class CheckListBlockDataConverter : BlockDataConverter<CheckListBlockData> {
    public CheckListBlockDataConverter(IUmbracoContextAccessor umbracoContextAccessor,
                                       IPublishedUrlProvider publishedUrlProvider)
        : base(umbracoContextAccessor, publishedUrlProvider) { }

    protected override string TypeId => "checklist";
}

public class CheckListBlockData {
    public List<CheckListItem> Items { get; set; }
}

public class CheckListItem {
    public string Text { get; set; }
    public bool Checked { get; set; }
}