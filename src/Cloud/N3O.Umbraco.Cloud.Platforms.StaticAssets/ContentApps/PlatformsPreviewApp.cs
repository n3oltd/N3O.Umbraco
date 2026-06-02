// TODO Migration Review: IContentAppFactory and ContentApp were removed in Umbraco 14.
// Content Apps are now registered via umbraco-package.json Bellissima extensions.
//
// This class previously registered a "Preview" content app tab for all content types
// that compose PlatformsConstants.Offerings.CompositionAlias.
//
// TODO Migration Review (v17 replacement): Create an App_Plugins/N3O.Umbraco.Cloud.Platforms.Preview/
// umbraco-package.json that registers a "contentApp" extension entry with an
// "entityType" condition matching the relevant document types.
namespace N3O.Umbraco.Cloud.Platforms;
