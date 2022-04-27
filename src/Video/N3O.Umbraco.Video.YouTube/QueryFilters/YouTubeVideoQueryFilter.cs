using N3O.Umbraco.Extensions;
using N3O.Umbraco.QueryFilters;
using N3O.Umbraco.Video.YouTube.Criteria;
using N3O.Umbraco.Video.YouTube.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Video.YouTube.QueryFilters {
    public class YouTubeVideoQueryFilter : QueryFilter<YouTubeVideo, YouTubeVideoCriteria> {
        public override IEnumerable<YouTubeVideo> Apply(IEnumerable<YouTubeVideo> q, YouTubeVideoCriteria criteria) {
            q = FilterKeyword(q, criteria);

            return q;
        }

        private IEnumerable<YouTubeVideo> FilterKeyword(IEnumerable<YouTubeVideo> q, YouTubeVideoCriteria criteria) {
            foreach (var keyword in criteria.Keyword.OrEmpty()) {
                q = q.Where(x => x.Keywords.Contains(keyword, StringComparer.InvariantCultureIgnoreCase));
            }

            return q;
        }
    }
}