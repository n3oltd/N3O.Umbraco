using N3O.Umbraco.Content;
using System;

namespace N3O.Umbraco.Redirects {
    public class Redirect : UmbracoContent {
        public int HitCount => GetValue<Redirect, int>(x => x.HitCount);
        public DateTime LastHitDate => GetValue<Redirect, DateTime>(x => x.LastHitDate);
        public bool Temporary => GetValue<Redirect, bool>(x => x.Temporary);
    }
}
