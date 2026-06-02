// SendingContentNotification and ContentVariantDisplay were removed in Umbraco 14
// when the Angular backoffice was replaced with Bellissima.
//
// This handler previously set staging/production URL info on the offering content display model
// when the editor was opened in the backoffice.
//
// TODO Migration Review (v17 replacement): Implement a Bellissima workspace view extension that fetches
// and displays offering URLs, or configure URL providers via IContentUrlProvider.
namespace N3O.Umbraco.Cloud.Platforms.Notifications;
