using System.Collections.Generic;

namespace N3O.Umbraco.Templates;

public interface IStyleContext {
    T Get<T>()  where T : TemplateStyle;
    IEnumerable<TemplateStyle> GetAll();
    bool Has(TemplateStyle style);
    void Pop(int count = 1);
    void Push(TemplateStyle style);
    void PushAll(IEnumerable<TemplateStyle> styles);
}
