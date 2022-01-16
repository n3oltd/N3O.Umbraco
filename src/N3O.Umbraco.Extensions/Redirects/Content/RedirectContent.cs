using N3O.Umbraco.Content;
using NodaTime;

namespace N3O.Umbraco.Redirects {
    public class RedirectContent : UmbracoContent<RedirectContent> {
        public int HitCount => GetValue(x => x.HitCount);
        public LocalDate LastHitDate => GetLocalDate(x => x.LastHitDate);
        public bool Temporary => GetValue(x => x.Temporary);
    }
}
