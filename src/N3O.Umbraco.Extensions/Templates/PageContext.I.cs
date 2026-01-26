using N3O.Umbraco.Pages;

namespace N3O.Umbraco.Templates;

public interface IPageContext {
    IPageViewModel Get();
    void SetViewModel(IPageViewModel pageViewModel);
}
