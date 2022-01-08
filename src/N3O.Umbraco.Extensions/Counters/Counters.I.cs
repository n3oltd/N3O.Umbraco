using System.Threading.Tasks;

namespace N3O.Umbraco.Counters {
    public interface ICounters {
        Task<long> NextAsync(string key, long startFrom = 1);
    }
}