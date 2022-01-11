using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Templates {
    public abstract class TemplateStyle : NamedLookup {
        protected TemplateStyle(string id, string name, string description) : base(id, name) {
            Description = description;
        }

        public string Description { get; }
        public abstract string Icon { get; }
    }

    public interface ITemplateStylesCollection { }

    public class TemplateStyles : DistributedLookupsCollection<TemplateStyle, ITemplateStylesCollection> { }
}