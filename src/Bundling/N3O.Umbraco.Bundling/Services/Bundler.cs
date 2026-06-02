using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Bundling;

// TODO Migration Review (RR-10): Smidge was removed in Umbraco 14, so this is a non-functional
// stub that throws at runtime. The N3O.Umbraco.Bundling project is orphaned (no consumers).
// Decision pending: delete the project, or replace with ES module-based bundling.
public class Bundler : IBundler {
    public Task<IEnumerable<string>> GenerateCssUrlsAsync() {
        throw new NotSupportedException("Smidge bundling was removed in Umbraco 14. Implement ES module-based bundling as a replacement.");
    }

    public Task<IEnumerable<string>> GenerateJsUrlsAsync() {
        throw new NotSupportedException("Smidge bundling was removed in Umbraco 14. Implement ES module-based bundling as a replacement.");
    }

    public IBundler RequiresCss(params string[] paths) {
        throw new NotSupportedException("Smidge bundling was removed in Umbraco 14. Implement ES module-based bundling as a replacement.");
    }

    public IBundler RequiresJs(params string[] paths) {
        throw new NotSupportedException("Smidge bundling was removed in Umbraco 14. Implement ES module-based bundling as a replacement.");
    }
}
