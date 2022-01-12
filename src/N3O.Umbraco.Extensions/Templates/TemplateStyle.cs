using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Templates {
    public abstract class TemplateStyle : NamedLookup {
        protected TemplateStyle(string id, string name, string description, string cssClass) : base(id, name) {
            Description = description;
            CssClass = cssClass;
        }

        public string Description { get; }
        public abstract string Icon { get; }
        public string CssClass { get; }
    }

    public interface ITemplateStylesCollection { }

    public class TemplateStyles : DistributedLookupsCollection<TemplateStyle, ITemplateStylesCollection> { }
}