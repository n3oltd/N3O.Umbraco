# PROGRESS.md

## Fix: LayoutBuilder view-path broken by Humanizer 3.x upgrade

### Changed file
`src/Blocks/N3O.Umbraco.Blocks.Perplex/Services/LayoutBuilder.cs`

### Background

`N3O.Umbraco.Extensions` was upgraded from **13.0.364** to **13.0.377**, which pulled in
`Humanizer.Core` **3.0.10** (up from **2.14.1**). This introduced a breaking change in the
`Pascalize` extension method that `LayoutBuilder.Build` was using to derive the view file path
from the layout display name.

### What changed in Humanizer

| Version | `Pascalize` regex | `"Alt 1".Pascalize()` |
|---|---|---|
| 2.14.1 | `(?:^|_| +)(.)` — any char after separator | `"Alt1"` |
| 3.0.10 | `(?:[ _-]+|^)([a-zA-Z])` — only **letters** after separator | `"Alt 1"` |

Humanizer 3.x changed the capture group from `.` (any character) to `[a-zA-Z]` (letters only).
As a result, a digit immediately following a space is no longer treated as a word start. A layout
named `"Alt 1"` would produce the path segment `"Alt 1"` (space preserved) instead of `"Alt1"`,
causing a 500 error because the partial view
`/Views/Blocks/HeroDonationFormBlock/Alt 2.cshtml` (with literal space) could not be found on
disk — the view files are named without spaces (`Alt1.cshtml`, `Alt2.cshtml`).

### Why renaming the C# definition would not fix it

The layout GUID is generated from the raw display name:

```csharp
UmbracoId.Generate(IdScope.BlockLayout, blockAlias, _name)
// e.g. "Alt 1" → deterministic GUID stored in the Umbraco database
```

Renaming `"Alt 1"` to `"Alt1"` in the definition produces a **different GUID**, so existing
published content in the database can no longer resolve its layout definition — the Perplex
backoffice shows "Layout Missing" on every affected block. A database migration would be required,
which is unacceptable for a package upgrade.

### The fix

`LayoutBuilder.Build` now uses an explicit `ToViewFileName` helper instead of `Humanizer.Pascalize`
to compute the view path segment. The helper replicates Humanizer 2.x behaviour — it uses the regex
`(?:^|[_\s-]+)(.)` which capitalises **any** character (letter or digit) that follows a word
separator, stripping the separator in the process.

- `"Alt 1"` → `"Alt1"` ✓
- `"Alt 2"` → `"Alt2"` ✓
- `"Default"` → `"Default"` ✓
- `"Alt1"` (no space) → `"Alt1"` ✓

The GUID is still generated from the raw `_name`, so existing database content is unaffected.
No view file renames, no definition renames, and no database changes are required.
