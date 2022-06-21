using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using Newtonsoft.Json;
using NJsonSchema.Generation;
using NodaTime;
using NodaTime.Serialization.JsonNet;
using System;

namespace N3O.Umbraco.Hosting;

public class NodaTimeFilter : TypeTransformationFilter {
    protected override void DoProcess(SchemaProcessorContext context) {
        // Pick a day greater than 12 to show ordering of month and days
        var instant = Instant.FromUtc(DateTime.UtcNow.Year, 4, 28, 12, 0);
        var zonedDateTime = instant.InZone(Timezones.Utc.Zone);

        var type = context.ContextualType.Type;
        object example = null;

        if (type.IsOfTypeOrNullableType<Instant>()) {
            example = instant;
        } else if (type.IsOfTypeOrNullableType<ZonedDateTime>()) {
            example = zonedDateTime;
        } else if (type.IsOfTypeOrNullableType<LocalDateTime>()) {
            example = zonedDateTime.LocalDateTime;
        } else if (type.IsOfTypeOrNullableType<LocalDate>()) {
            example = zonedDateTime.LocalDateTime.Date;
        } else if (type.IsOfTypeOrNullableType<LocalTime>()) {
            example = zonedDateTime.LocalDateTime.TimeOfDay;
        }

        if (example != null) {
            var settings = new JsonSerializerSettings();
            settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
            
            var exampleJson = JsonConvert.SerializeObject(example, settings);

            // Remove leading and trailing quotes
            exampleJson = exampleJson.Substring(1, exampleJson.Length - 2);

            ModelAsString(exampleJson);
        }
    }
}
