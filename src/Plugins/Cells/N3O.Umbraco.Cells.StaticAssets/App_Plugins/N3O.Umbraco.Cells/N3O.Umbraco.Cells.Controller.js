angular.module("umbraco").controller("N3O.Umbraco.Cells",
    function ($scope, assetsService) {
        $scope.makeId = function () {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < 10; i++) {
                text += possible.charAt(Math.floor(Math.random() * possible.length));
            }

            return text;
        };

        $scope.uniqueId = $scope.makeId();

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Cells/handsontable.full.min.css");
        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Cells/N3O.Umbraco.Cells.css");

        window.setTimeout(function() {
            const container = document.querySelector('#' + $scope.uniqueId);
            let data = $scope.model.value;

            const localConfig = JSON.parse($scope.model.config.gridConfiguration);

            if (!data) {
                data = localConfig.data;
            }

            const globalConfig = {
                licenseKey: 'non-commercial-and-evaluation',
                height: 'auto',
                width: 'auto',
                data: data,
                afterChange: function (change, source) {
                    if (source !== 'loadData') {
                        $scope.model.value = hot.getData();
                    }
                }
            };

            const hot = new Handsontable(container, { ...localConfig, ...globalConfig });
        });
    }, 2000);
