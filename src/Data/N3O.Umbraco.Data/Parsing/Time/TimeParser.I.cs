using System.Collections.Generic;
using NodaTime;
using NodaTime.Text;

namespace N3O.Umbraco.Data.Parsing {
    public interface ITimeParser : IDataTypeParser<LocalTime?> {
        IReadOnlyList<LocalTimePattern> Patterns { get; }
    }
}