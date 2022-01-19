using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.QueryFilters {
    public abstract class ContentQueryFilter<TCriteria> : IContentQueryFilter<TCriteria> {
        public abstract IEnumerable<T> Apply<T>(IEnumerable<T> q, TCriteria criteria) where T : IPublishedContent;

        protected IEnumerable<T> Where<T>(IEnumerable<T> q, Func<T, bool> predicate) where T : IPublishedContent {
            return q.Where(predicate);
        }
        
        protected IEnumerable<T> Where<T, TIntermediate>(IEnumerable<T> q, Func<TIntermediate, bool> predicate)
            where T : IPublishedContent {
            return q.Where(x => predicate(x.As<TIntermediate>()));
        }
    }
}