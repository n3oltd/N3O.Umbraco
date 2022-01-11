using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Templates {
    public class TemplateText : ITemplateText {
        private readonly IStringLocalizer _stringLocalizer;
        private string _templateName;

        public TemplateText(IStringLocalizer stringLocalizer) {
            _stringLocalizer = stringLocalizer;
        }
        
        public string Get(string s) {
            if (!_templateName.HasValue()) {
                throw new Exception($"Must set template name before calling {nameof(Get)}");
            }

            return _stringLocalizer.Get(TextFolders.Template, _templateName, s);
        }

        public void SetTemplateName(string name) {
            _templateName = name;
        }
    }
}