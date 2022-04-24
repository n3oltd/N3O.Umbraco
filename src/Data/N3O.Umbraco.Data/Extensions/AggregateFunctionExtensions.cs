using N3O.Umbraco.Data.Attributes;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using System.Linq;

namespace N3O.Umbraco.Data.Extensions {
    public static class AggregationFunctionExtensions {
        public static bool Applies(this AggregationFunction aggregateFunction, Column column) {
            var attribute = column.Attributes.OfType<FooterAttribute>().SingleOrDefault();

            var applies = attribute?.FooterFunction == aggregateFunction;

            return applies;
        }
    }
}