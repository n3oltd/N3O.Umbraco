using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System.Linq;

namespace N3O.Umbraco.Data.Converters {
    public class TitleMetadataColumnHeading : IColumnHeading {
        public string GetText(IFormatter formatter, int? columnIndex, Cell cell) {
            return (string) cell.Metadata[DataConstants.MetadataKeys.Cells.Title].Single();
        }
    }
}