using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Templates {
    public class StyleContext : IStyleContext {
        private Stack<TemplateStyle> _stack;

        public T Get<T>() where T : TemplateStyle {
            foreach (var style in Stack) {
                if (style is T typedStyle) {
                    return typedStyle;
                }
            }

            return default;
        }

        public bool Has(TemplateStyle style) {
            return Stack.Any(x => x == style);
        }

        public void Pop(int count = 1) {
            for (var i = 0; i < count; i++) {
                Stack.Pop();
            }
        }

        public void Push(TemplateStyle style) {
            Stack.Push(style);
        }

        public void PushAll(IEnumerable<TemplateStyle> styles) {
            styles.OrEmpty().Do(Push);
        }

        private Stack<TemplateStyle> Stack {
            get {
                return _stack ??= new Stack<TemplateStyle>();
            }
        }
    }
}