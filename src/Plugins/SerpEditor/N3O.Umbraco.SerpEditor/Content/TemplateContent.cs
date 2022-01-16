using N3O.Umbraco.Content;

namespace N3O.Umbraco.SerpEditor.Content {
    public class TemplateContent : UmbracoContent<TemplateContent> {
        public string TitleSuffix => GetValue(x => x.TitleSuffix);
    }
}