angular.module("umbraco")
    .controller("N3O.Umbraco.SerpEditor",
        function ($scope, assetsService, editorState) {
            if ($scope.model.value) {
                $scope.title = $scope.model.value.title;
                $scope.description = $scope.model.value.description;
            } else {
                $scope.title = "";
                $scope.description = "";
            }

            $scope.maxCharsTitle = 60;
            $scope.maxCharsDescription = 160;
            $scope.titleSuffix = '';

            if ($scope.model.config) {
                if ($scope.model.config.maxCharsTitle !== '' && parseInt($scope.model.config.maxCharsTitle) > 0) {
                    $scope.maxCharsTitle = parseInt($scope.model.config.maxCharsTitle);
                }
                if ($scope.model.config.maxCharsDescription !== '' && parseInt($scope.model.config.maxCharsDescription) > 0) {
                    $scope.maxCharsDescription = parseInt($scope.model.config.maxCharsDescription);
                }
                if ($scope.model.config.titleSuffix !== '') {
                    $scope.titleSuffix = $scope.model.config.titleSuffix;
                }
            }

            $scope.model.value = {title: $scope.title, description: $scope.description};

            $scope.$watch("title", function () {
                $scope.UpdateModel();
            });

            $scope.$watch("description", function () {
                $scope.UpdateModel();
            });

            $scope.UpdateModel = function () {
                $scope.model.value = {title: $scope.title, description: $scope.description};
            };

            $scope.GetUrl = function () {
                if (!editorState || !editorState.current) {
                    return '';
                }

                var allUrls = editorState.current.urls;

                if (!allUrls) {
                    return '';
                }

                var url = '';
                for (var i = 0; i < allUrls.length; i++) {
                    if (!$scope.model.culture || allUrls[i].culture == $scope.model.culture) {
                        url = allUrls[i].text;
                        break;
                    }
                }

                if (url.indexOf('http://') == 0 || url.indexOf('https://') == 0) {
                    return url;
                }

                return $scope.ProtocolAndHost() + url;
            };

            $scope.ProtocolAndHost = function () {
                var http = location.protocol;
                var slashes = http.concat("//");
                return slashes.concat(window.location.hostname);
            };

            assetsService.loadCss("~/App_Plugins/N3O.Umbraco.SerpEditor/N3O.Umbraco.SerpEditor.css");
        });
