using NodaTime;
using NodaTime.Text;
using System.Collections.Generic;

namespace N3O.Umbraco.Data.Parsing;

public interface IDateParser : IDataTypeParser<LocalDate?> {
    IReadOnlyList<LocalDatePattern> Patterns { get; }
}
