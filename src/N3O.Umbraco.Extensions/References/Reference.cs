using System.Collections.Generic;

namespace N3O.Umbraco.References {
    public class Reference : Value, IReference {
        public Reference(ReferenceType type, long number) {
            Type = type;
            Number = number;
        }

        public ReferenceType Type { get; }
        public long Number { get; }
        public string Text => $"{Type.Prefix}{Number}";

        public override string ToString() {
            return Text;
        }

        protected override IEnumerable<object> GetAtomicValues() {
            yield return Text;
        }
    }
}