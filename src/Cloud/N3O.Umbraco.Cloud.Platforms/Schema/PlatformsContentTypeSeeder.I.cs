using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms;

public interface IPlatformsContentTypeSeeder {
    // The content types platforms owns, in the order they are seeded. Anything not named here reaches a site
    // some other way, so callers reporting on schema can tell the two apart
    IReadOnlyList<string> Aliases { get; }
    void Seed();
}
