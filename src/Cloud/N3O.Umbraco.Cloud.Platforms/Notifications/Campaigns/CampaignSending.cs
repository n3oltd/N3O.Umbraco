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
// 3. Crowdfunding tab visibility for new campaigns: IMPLEMENTED as the
//    N3O.WorkspaceContext.CrowdfundingVisibility extension (frontend/crowdfunding-visibility). It denies
//    the crowdfunding tab's properties through the document workspace propertyViewGuard while the document
//    is new (getIsNew), reproducing the v13 ContentSavedState.NotCreated behaviour. The crowdfunding
//    properties therefore appear only after the campaign has been saved (created). NOTE: v17 has no API to
//    hide a property-group TAB itself, only its properties, so the (empty) crowdfunding tab still renders on
//    a brand-new campaign until it is first saved.
namespace N3O.Umbraco.Cloud.Platforms.Notifications;
