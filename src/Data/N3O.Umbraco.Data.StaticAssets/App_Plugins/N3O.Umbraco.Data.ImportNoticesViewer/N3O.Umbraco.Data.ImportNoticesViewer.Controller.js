angular.module("umbraco").controller("N3O.Umbraco.Data.ImportNoticesViewer",
    function($scope, assetsService) {
        $scope.errors = $scope.model.value == null ? null : $scope.model.value.errors;
        $scope.warnings = $scope.model.value == null ? null : $scope.model.value.warnings;
    
		assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Data.ImportNoticesViewer/N3O.Umbraco.Data.ImportNoticesViewer.css");
    });
