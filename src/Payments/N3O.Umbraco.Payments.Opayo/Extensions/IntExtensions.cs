using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Opayo.Extensions {
    public static class IntExtensions {
        public static int? GetColorDepth(this int? colorDepth) {
            if (colorDepth == null) {
                return null;
            }
            
            var colorsDepth = new int[] {1, 4, 8, 15, 16, 24, 32, 48};
            var selectedColor = GetClosetItem(colorsDepth, colorDepth.GetValueOrThrow(), (a, b) => Math.Abs(a - b));

            return selectedColor;
        }

        private static T GetClosetItem<T>(IEnumerable<T> items, T item, Func<T, T, int> getDiffernce, T defaultValue = default ) {
            T selected = defaultValue;
            var difference = int.MaxValue;
            foreach (var i in items) {
                var newDiffernce = getDiffernce(i,item);
                
                if (newDiffernce < difference) {
                    selected = i;
                    difference = newDiffernce;
                }
            }

            return selected;
        }
    }
}