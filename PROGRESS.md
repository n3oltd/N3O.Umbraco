# YouTube Video Tag Helper — Flexible Attributes

**Date:** 2026-06-17
**Work issue:** n3oltd/work#2427 — *[Bug] Youtube Video block is not rendering during campaign page creation on Umbraco* (afhuk, area/sites)
**Branch:** `fix/youtube-video-tag-helper-flexible`

---

## Goal

Make `<n3o-youtube-video>` the single, central way all sites embed YouTube videos, so the
watch-URL → embed-URL handling lives in one place and each site can pass whatever iframe
attributes it needs.

## Background

YouTube **watch** URLs (and `youtu.be` / `shorts` forms) set `X-Frame-Options: sameorigin`
and cannot be shown in an `<iframe>`. Sites that drop a raw watch URL into an iframe `src`
fail to render the video (work#2427, AFH). The fix is to always render the `embed` form,
derived from the video id, centrally in `N3O.Umbraco.Video.YouTube`.

## Changes

- `TagHelpers/YouTubeVideoTagHelper.cs` — forwards every attribute supplied on the element
  (e.g. `width`, `height`, `title`, `class`, `allow`, `referrerpolicy`) onto the generated
  iframe. The iframe `src` is always built from the extracted video id; the hardcoded
  `frameborder`, `allowfullscreen` and responsive `style` are applied only when the caller
  did not supply them. The responsive wrapper `div` is unchanged.
- `Extensions/StringExtensions.cs` — `GetYouTubeVideoId()` now also recognises `shorts/` and
  `live/` URLs (in addition to the existing `watch?v=`/`&v=`, `youtu.be/`, `embed/`, `v/`).

## Backward compatibility

Existing usages that pass only `video-url` (e.g. AFH V1 `VideoEmbedBlock` views) render
identically: no extra attributes to forward, so the same responsive wrapper + iframe with the
same defaults is produced.

## Verification

- `N3O.Umbraco.Video.YouTube` builds with 0 errors.
- AFH V2 consumes this via its local project reference and switches its video blocks to
  `<n3o-youtube-video>` (see the AFH site PR linked to the same work issue).
