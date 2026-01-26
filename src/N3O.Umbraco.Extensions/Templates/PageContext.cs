using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Pages;
using System;

namespace N3O.Umbraco.Templates;

public class PageContext : IPageContext {
    private IPageViewModel _pageViewModel;
    
    public IPageViewModel Get() {
        return _pageViewModel;
    }

    public void SetViewModel(IPageViewModel pageViewModel) {
        _pageViewModel = pageViewModel;
    }
}
