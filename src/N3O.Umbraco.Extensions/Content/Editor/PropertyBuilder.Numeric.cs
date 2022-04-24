namespace N3O.Umbraco.Content {
    public class NumericPropertyBuilder : PropertyBuilder {
        public void SetDecimal(decimal? value) {
            Value = value;
        }

        public void SetInteger(int? value) {
            Value = value;
        }
        
        public void SetInteger(long? value) {
            SetInteger((int?) value);
        }
    }
}