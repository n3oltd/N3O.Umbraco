# Progress

## Checkout webhook: tag donor's language instead of a static site env var

### What
`CheckoutWebhookTransform.TransformTags` now derives the `SiteLanguageTag` from the
language the donor actually used for the checkout, falling back to the previous behaviour
when unavailable.

- `Checkout` entity gained a `Culture` property (the .NET culture code, e.g. `en-GB`).
- It is captured at checkout creation (`Checkout.CreateAsync`) from
  `LocalizationSettings.CultureCode` (the current request's thread culture). The manual
  `Checkout.Create(...)` factory takes an optional `culture` argument.
- The transform maps that culture code to an English language name (`en-GB` → "English",
  `ar` → "Arabic") via the neutral culture's `EnglishName`, and falls back to
  `Site.Language` (the `N3O_SiteLanguage` env var) when the checkout has no culture or the
  code is unrecognised.

### Why
The previous code tagged every webhook with `Site.Language`, a single site-wide
environment variable. On a multilingual Umbraco site (one deployment, many languages) that
cannot represent the language an individual donor used. The correct language is only known
during the donor's request; by the time the webhook transform runs it executes inside a
background job with no request culture, so the value must travel with the checkout.

### How it works / what we achieve
- Culture is captured during the checkout-creating page request, where Umbraco has set the
  thread culture from the domain/language binding — so it reflects the donor's language.
- Stored as a plain string on the entity, it round-trips through the entity's JSON
  persistence and survives into the cultureless background job that dispatches the webhook.
- Result: webhooks carry the donor's actual language (e.g. "English" / "Arabic") instead of
  a static site-wide value, while older/in-flight checkouts and unknown cultures degrade
  gracefully to the existing env-var behaviour (no regression).

### Notes
- Name mapping uses `CultureInfo.EnglishName`, which is locale-invariant. Common languages
  (English, Arabic) match exactly; verify the wording for any other configured languages
  against the receiver's expectations.
- This area is expected to be superseded by the new donation widget; the change is
  intentionally minimal.
