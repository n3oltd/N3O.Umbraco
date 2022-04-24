using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Models {
    public class Row : Value {
        public Row(IEnumerable<Cell> cells) {
            Cells = cells.ToList();
        }

        public IReadOnlyList<Cell> Cells { get; }

        protected override IEnumerable<object> GetAtomicValues() {
            foreach (var cell in Cells) {
                yield return cell;
            }
        }

        public override int GetHashCode() {
            var hashCode = new HashCode();

            foreach (var cell in Cells) {
                hashCode.Add(cell.GetHashCode());
            }

            return hashCode.ToHashCode();
        }
    }
}