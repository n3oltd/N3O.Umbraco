using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Models {
    public abstract class Cell : Value {
        protected Cell(DataType type, object value, Type targetType) {
            Type = type;
            Value = value;
            TargetType = targetType;
        }

        public DataType Type { get; }
        public object Value { get; }
        public Type TargetType { get; }

        public string ToString(IFormatter formatter) {
            return Type.ConvertToText(formatter, Value);
        }
    }

    public class Cell<T> : Cell {
        protected internal Cell(DataType type, T value, Type targetType) : base(type, value, targetType) {
            Value = value;
        }

        public new T Value { get; }
    }
}