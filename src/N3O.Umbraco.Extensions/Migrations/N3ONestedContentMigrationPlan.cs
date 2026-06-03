// ─────────────────────────────────────────────────────────────────────────────
// MOVED TO STANDALONE CLI — this migration no longer runs on application startup.
//
// The Nested Content → Block List data conversion has been extracted to an
// independent console tool so it can be run OFFLINE against the database (and
// tested against an Umbraco 13 site without upgrading):
//
//     N3O.Umbraco.NestedContentMigration.Cli   (a standalone project, a sibling
//     folder of this repository — see its README.md)
//
// The PackageMigrationPlan below is commented out so Umbraco no longer
// auto-discovers/runs it. Original preserved for reference only.
// ─────────────────────────────────────────────────────────────────────────────

/*
using Umbraco.Cms.Core.Packaging;

namespace N3O.Umbraco.Migrations;

public class N3ONestedContentMigrationPlan : PackageMigrationPlan {
    public N3ONestedContentMigrationPlan() : base("N3O.Umbraco.NestedToBlockList") { }

    protected override void DefinePlan() {
        To<NestedContentToBlockListMigration>("2026-NestedContent-v1");
    }
}
*/
