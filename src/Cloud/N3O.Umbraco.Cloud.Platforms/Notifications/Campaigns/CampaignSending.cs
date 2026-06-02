// SendingContentNotification, ContentVariantDisplay, ContentPropertyDisplay and Tab<T> were
// TODO Migration Review: all removed in Umbraco 14 when the Angular backoffice was replaced with Bellissima.
//
// This handler previously:
//   1. Injected embed-code HTML into read-only properties when the campaign editor opened
//   2. Set staging/production URL info on the content display model
//   3. Hid "crowdfunding" tabs for unpublished campaigns
//
// TODO Migration Review (v17 replacement): Implement equivalent functionality using:
//   - A Bellissima workspace view extension (Lit web component) to show embed codes
//   - IContentUrlProvider or equivalent for custom URL display in the backoffice
//   - Workspace conditions to hide sections based on content state
//
// Register these as Bellissima UI extensions in an App_Plugins umbraco-package.json manifest.
namespace N3O.Umbraco.Cloud.Platforms.Notifications;
