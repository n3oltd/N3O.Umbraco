using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Redirects {
    public class Redirect : UmbracoContent<Redirect> {
        public int HitCount => GetValue(x => x.HitCount);
        public DateTime LastHitDate => GetValue(x => x.LastHitDate);
        public bool Temporary => GetValue(x => x.Temporary);
    }
}
