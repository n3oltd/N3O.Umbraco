angular.module("umbraco")
    .controller("N3O.Umbraco.Crowdfunding.Statistics", function ($scope, editorState, contentResource, assetsService) {
        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });