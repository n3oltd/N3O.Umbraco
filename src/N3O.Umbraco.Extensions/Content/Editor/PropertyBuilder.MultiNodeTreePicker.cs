using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public class MultiNodeTreePickerPropertyBuilder : PropertyBuilder {
        public void SetContent<T>(IEnumerable<T> values) where T : IUmbracoContent {
            SetContent(values.OrEmpty().ToArray());
        }
        
        public void SetContent<T>(params T[] values) where T : IUmbracoContent {
            SetContent(values.Select(x => x?.Content()).ToArray());
        }
        
        public void SetContent(IEnumerable<IPublishedContent> values) {
            SetContent(values.OrEmpty().ToArray());
        }
        
        public void SetContent(params IPublishedContent[] values) {
            var documentUdis = values.ExceptNull().Select(x => Udi.Create("document", x.Key).ToString()).ToList();
            
            if (documentUdis.IsSingle()) {
                Value = documentUdis.Single();
            } else {
                Value = string.Join(",", documentUdis);
            }
        }
    }
}