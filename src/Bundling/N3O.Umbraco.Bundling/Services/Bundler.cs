using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Bundling;

// Smidge was removed in Umbraco 14. This implementation is a non-functional stub.
// TODO: Replace with ES module-based bundling.
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
