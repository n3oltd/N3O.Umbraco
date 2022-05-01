using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Models {
    public class TextColumnHeading : IColumnHeading {
        private readonly Func<ITextFormatter, string> _getText;
        private string _text;

        public TextColumnHeading(Func<ITextFormatter, string> getText) {
            _getText = getText;
        }

        public string GetText(IFormatter formatter, int? columnIndex, Cell cell) {
            _text ??= _getText(formatter.Text);
        
            if (columnIndex > 0) {
                return $"{_text} {columnIndex + 1}";
            } else {
                return _text;
            }
        }
    }
}