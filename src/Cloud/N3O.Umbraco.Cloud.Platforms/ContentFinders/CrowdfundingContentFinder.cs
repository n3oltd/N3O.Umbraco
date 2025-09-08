// using N3O.Umbraco.Content;
// using System.Threading.Tasks;
// using Umbraco.Cms.Core.Routing;
//
// namespace N3O.Umbraco.Crowdfunding;
//
// // Find campaigns or designations under the donate URL 
// public class CrowdfundingContentFinder : IContentFinder {
//     private readonly IContentLocator _contentLocator;
//
//     public CrowdfundingContentFinder(IContentLocator contentLocator) {
//         _contentLocator = contentLocator;
//     }
//     
//     public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
//         var crowdfundingPath = CrowdfundingPathParser.ParseUri(_contentLocator, request.Uri);
//
//         if (crowdfundingPath.HasValue()) {
//             request.SetPublishedContent(_contentLocator.Single<HomePageContent>().Content());
//
//             return Task.FromResult(true);
//         } else {
//             return Task.FromResult(false);
//         }
//     }
// }