using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Localization {
    public class TextContainer : UmbracoContent {
        public IEnumerable<TextResource> Resources => GetValue<TextContainer, IEnumerable<TextResource>>(x => x.Resources);
    }
}
