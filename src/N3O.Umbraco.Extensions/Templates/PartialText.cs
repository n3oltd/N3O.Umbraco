using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Templates {
    public class PartialText : IPartialText {
        private readonly IStringLocalizer _stringLocalizer;
        private string _templateName;

        public PartialText(IStringLocalizer stringLocalizer) {
            _stringLocalizer = stringLocalizer;
        }
        
        public string Get(string s) {
            if (!_templateName.HasValue()) {
                throw new Exception($"Must set partial name before calling {nameof(Get)}");
            }

            return _stringLocalizer.Get(TextFolders.Partial, _templateName, s);
        }

        public void SetPartialName(string name) {
            _templateName = name;
        }
    }
}