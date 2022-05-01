angular.module("umbraco").controller("N3O.Umbraco.Data.ImportFieldsEditor",
    function($scope, assetsService) {
		$scope.toggleIgnore = function(index) {
			alert('TODO');
		}

		assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Data.ImportFieldsEditor/N3O.Umbraco.Data.ImportFieldsEditor.css");
    });
