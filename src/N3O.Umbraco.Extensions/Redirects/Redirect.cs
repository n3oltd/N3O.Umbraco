using NodaTime;
using System;

namespace N3O.Umbraco.Redirects {
    public class Redirect : Value {
        public Redirect(Guid id, int hitCount, LocalDate lastHitDate, bool temporary, string url) {
            Id = id;
            HitCount = hitCount;
            LastHitDate = lastHitDate;
            Temporary = temporary;
            Url = url;
        }

        public Guid Id { get; }
        public int HitCount { get; }
        public LocalDate LastHitDate { get; }
        public bool Temporary { get; }
        public string Url { get; }
    }
}