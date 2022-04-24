using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Extensions {
    public static class CellExtensions {
        public static T GetValue<T>(this Cell cell) {
            return (T) cell.Value;
        }
    }
}