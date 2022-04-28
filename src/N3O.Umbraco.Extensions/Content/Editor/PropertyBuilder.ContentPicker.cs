using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public class ContentPickerPropertyBuilder : PropertyBuilder {
        public void SetContent(IEnumerable<IContent> values) {
            SetContent(values.OrEmpty().ToArray());
        }

        public void SetContent(params IContent[] values) {
            SetContent(values.ExceptNull().Select(x => x.Key).ToArray());
        }

        public void SetContent<T>(IEnumerable<T> values) where T : IUmbracoContent {
            SetContent(values.OrEmpty().ToArray());
        }

        public void SetContent<T>(params T[] values) where T : IUmbracoContent {
            SetContent(values.Select(x => x?.Content()).ExceptNull().Select(x => x.Key).ToArray());
        }

        public void SetContent(IEnumerable<IPublishedContent> values) {
            SetContent(values.OrEmpty().ToArray());
        }

        public void SetContent(params IPublishedContent[] values) {
            SetContent(values.ExceptNull().Select(x => x.Key).ToArray());
        }

        public void SetContent(IEnumerable<Guid> values) {
            SetContent(values.OrEmpty().ToArray());
        }
        
        public void SetContent(params Guid[] values) {
            var documentUdis = values.ExceptNull().Select(x => Udi.Create("document", x).ToString()).ToList();

            if (documentUdis.IsSingle()) {
                Value = documentUdis.Single();
            } else {
                Value = string.Join(",", documentUdis);
            }
        }
    }
}