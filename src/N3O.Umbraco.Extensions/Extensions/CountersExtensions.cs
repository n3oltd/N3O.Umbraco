using N3O.Umbraco.Counters;
using System.Threading.Tasks;

namespace N3O.Umbraco.Extensions {
    public static class CountersExtensions {
        public static async Task<Reference> NextReference(this ICounters counters, ReferenceType referenceType) {
            var number = await counters.NextAsync(referenceType.Id, referenceType.StartFrom);

            return new Reference(referenceType, number);
        }
    }
}