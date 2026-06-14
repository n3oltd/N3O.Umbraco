// v17 status (Bellissima migration):
//
// 1. Embed codes (donationFormEmbedCode / donationButtonEmbedCode / donationPopupEmbedCode):
//    These are stored as Umbraco content properties on campaign document types and appear
//    automatically in the Content tab alongside other properties (Notes, Target, etc.) — no
//    handler required.
//
// 2. Staging/production URL display: IMPLEMENTED as the N3O.WorkspaceInfoApp.PlatformsUrls
//    panel in the Info tab (platforms-urls-info-app.ts / PlatformsBackOfficeController.GetContentUrls).
//
// 3. Crowdfunding tab visibility for unpublished campaigns: NOT YET IMPLEMENTED. Requires a
//    custom Bellissima workspace condition based on the content's publish state.
//    Deferred.
namespace N3O.Umbraco.Cloud.Platforms.Notifications;
