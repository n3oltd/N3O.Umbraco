// v17 status (Bellissima migration):
//
// 1. Embed codes (donationFormEmbedCode / donationButtonEmbedCode / donationPopupEmbedCode):
//    These are stored as Umbraco content properties and are displayed automatically by the v17
//    backoffice editor — no handler required for this functionality.
//
// 2. Staging/production URL display: NOT YET IMPLEMENTED. Requires a new workspace view
//    extension (Lit) that fetches and displays the platforms URLs for the current campaign.
//    Deferred — needs a backend endpoint and knowledge of the URL scheme.
//
// 3. Crowdfunding tab visibility for unpublished campaigns: NOT YET IMPLEMENTED. Requires a
//    custom Bellissima workspace condition based on the content's publish state.
//    Deferred.
namespace N3O.Umbraco.Cloud.Platforms.Notifications;
