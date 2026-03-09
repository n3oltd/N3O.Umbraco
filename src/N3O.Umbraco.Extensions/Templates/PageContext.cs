using N3O.Umbraco.Pages;

namespace N3O.Umbraco.Templates;

public class PageContext : IPageContext {
    private IPageViewModel _pageViewModel;
    
    public IPageViewModel GetViewModel() {
        return _pageViewModel;
    }

    public void SetViewModel(IPageViewModel pageViewModel) {
        _pageViewModel = pageViewModel;
    }
}
