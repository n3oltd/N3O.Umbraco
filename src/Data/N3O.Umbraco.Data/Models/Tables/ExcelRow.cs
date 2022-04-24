using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Models {
    public class ExcelRow : Row {
        public ExcelRow(IEnumerable<ExcelCell> cells) : base(cells) {
            Cells = cells.ToList();
        }

        public new IEnumerable<ExcelCell> Cells { get; }
    }
}