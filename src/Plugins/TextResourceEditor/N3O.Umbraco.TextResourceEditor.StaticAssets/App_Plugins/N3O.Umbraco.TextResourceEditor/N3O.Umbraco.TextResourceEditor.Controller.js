angular.module("umbraco").controller("N3O.Umbraco.TextResourceEditor",
    function($scope, assetsService) {
		$scope.deleteEntry = function(index) {
			if (confirm("Are you sure you wish to delete this entry?")) {
				$scope.model.value.splice(index, 1);
			}
		}

		assetsService.loadCss("~/App_Plugins/N3O.Umbraco.TextResourceEditor/N3O.Umbraco.TextResourceEditor.css");
    });
