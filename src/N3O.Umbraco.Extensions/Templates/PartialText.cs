using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Templates;

public class PartialText : IPartialText {
    private readonly IStringLocalizer _stringLocalizer;
    private string _partialName;

    public PartialText(IStringLocalizer stringLocalizer) {
        _stringLocalizer = stringLocalizer;
    }
    
    public string Get(string s) {
        if (!_partialName.HasValue()) {
            throw new Exception($"Must set partial name before calling {nameof(Get)}");
        }

        return _stringLocalizer.Get(TextFolders.Partial, _partialName, s);
    }

    public void SetPartialName(string name) {
        _partialName = name;
    }
}
