using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Templates;

public class StyleContext : IStyleContext {
    private Stack<ITemplateStyle> _stack;

    public T Get<T>() where T : ITemplateStyle {
        foreach (var style in Stack) {
            if (style is T typedStyle) {
                return typedStyle;
            }
        }

        return default;
    }

    public IEnumerable<ITemplateStyle> GetAll() {
        var seenTypes = new List<Type>();
        var list = new List<ITemplateStyle>();

        foreach (var style in Stack) {
            if (seenTypes.Contains(style.GetType())) {
                continue;
            }
            
            list.Add(style);
            seenTypes.Add(style.GetType());
        }

        return list;
    }

    public bool Has(ITemplateStyle style) {
        return GetAll().Any(x => x.Id.EqualsInvariant(style.Id));
    }

    public void Pop(int count = 1) {
        for (var i = 0; i < count; i++) {
            Stack.Pop();
        }
    }

    public void Push(ITemplateStyle style) {
        Stack.Push(style);
    }

    public void PushAll(IEnumerable<ITemplateStyle> styles) {
        styles.OrEmpty().Do(Push);
    }

    private Stack<ITemplateStyle> Stack {
        get {
            return _stack ??= new Stack<ITemplateStyle>();
        }
    }
}
