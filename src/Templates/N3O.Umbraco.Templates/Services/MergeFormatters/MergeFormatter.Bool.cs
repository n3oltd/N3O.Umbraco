using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates {
    public class BoolMergeFormatter : MergeFormatter<bool?> {
        public BoolMergeFormatter(IFormatter formatter) : base(formatter) { }
    
        protected override string Format(bool? value) {
            return value.ToYesNoString(Formatter.Text);
        }
    }
}
