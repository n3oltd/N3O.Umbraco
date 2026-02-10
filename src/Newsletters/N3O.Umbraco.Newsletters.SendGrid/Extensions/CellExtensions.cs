using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Newsletters.SendGrid.Models;
using NodaTime;
using Umbraco.Extensions;

namespace N3O.Umbraco.Newsletters.SendGrid.Extensions; 

public static class CellExtensions {
    public static object GetValue(this Cell cell) {
        if (cell.Type == DataTypes.Date) {
            var localDate = (LocalDate?) cell.Value;

            return localDate;
        } else if (cell.Type == DataTypes.Integer) {
            var integer = (int?) cell.Value;

            return integer;
        } else if (cell.Type == DataTypes.Decimal) {
            var @decimal = (decimal?) cell.Value;

            return @decimal;
        } else {
            return cell.Value.ToString().Truncate(50);
        }
    }
}