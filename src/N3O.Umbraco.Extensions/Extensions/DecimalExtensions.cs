namespace N3O.Umbraco.Extensions {
    public static class DecimalExtensions {
        public static bool HasValue(this decimal? value) {
            if (value == null) {
                return false;
            }

            return true;
        }
    }
}
