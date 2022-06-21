using N3O.Umbraco.Attributes;
using System.Collections.Generic;

namespace N3O.Umbraco.Video.YouTube.Criteria;

public class YouTubeVideoCriteria {
    [Name("Keyword")]
    public IEnumerable<string> Keyword { get; set; }
}
