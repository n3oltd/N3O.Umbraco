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
            if (_text == null) {
                _text = _getText(formatter.Text);
            }

            if (columnIndex == null) {
                return _text;
            }

            return $"{_text} {columnIndex.Value + 1}";
        }
    }
}