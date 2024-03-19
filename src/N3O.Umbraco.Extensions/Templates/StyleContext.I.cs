using System.Collections.Generic;

namespace N3O.Umbraco.Templates;

public interface IStyleContext {
    T Get<T>()  where T : ITemplateStyle;
    IEnumerable<ITemplateStyle> GetAll();
    bool Has(ITemplateStyle style);
    void Pop(int count = 1);
    void Push(ITemplateStyle style);
    void PushAll(IEnumerable<ITemplateStyle> styles);
}
