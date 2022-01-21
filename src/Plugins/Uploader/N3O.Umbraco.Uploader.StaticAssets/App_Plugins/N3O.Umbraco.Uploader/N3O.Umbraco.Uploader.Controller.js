angular.module("umbraco").controller("N3O.Umbraco.Uploader",
    function ($scope, assetsService) {
        $scope.uploadInProgress = false;
        $scope.errorMessage = null;

        $scope.makeId = function () {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < 10; i++) {
                text += possible.charAt(Math.floor(Math.random() * possible.length));
            }

            return text;
        };

        $scope.copytoClipboard = function (text) {
            var $temp = $("<input>");
            $("body").append($temp);
            $temp.val(text).select();
            document.execCommand("copy");
            $temp.remove();
        };

        $scope.uniqueId = $scope.makeId();
        $scope.imageMode = !($scope.model.config.imagesOnly == "0");

        $scope.startOver = function (showConfirmPrompt) {
            if (showConfirmPrompt && !confirm("Are you sure?")) {
                return;
            }

            $scope.model.value = null;
            $scope.errorMessage = null;
        };

        $scope.processResponse = function (errorMessage, json) {
            if (errorMessage === null) {
                var response = JSON.parse(json);
                
                $scope.model.value = {
                    urlPath: response.urlPath,
                    mediaId: response.mediaId,
                    extension: response.extension,
                    sizeMb: response.sizeMb,
                    filename: response.filename,
                };
            } else {
                $scope.errorMessage = errorMessage;
            }

            $scope.uploadInProgress = false;
        };

        $scope.loadMediaById = function () {
            if (!$scope.mediaId || $scope.mediaId.length !== 12) {
                return;
            }

            $.ajax({
                type: "GET",
                url: "/umbraco/backoffice/uploader/media/" + $scope.mediaId,
                success: function (json) { $scope.processResponse(null, json); },
                error: function () { $scope.processResponse("No media found with the specified ID"); }
            });
        };

        assetsService
            .load([
                "~/App_Plugins/N3O.Umbraco.Uploader/formstone/core.js",
                "~/App_Plugins/N3O.Umbraco.Uploader/formstone/upload.js"
            ])
            .then(function () {
                window.setTimeout(function () {
                    $("#" + $scope.uniqueId + " .upload").upload({
                        action: "/umbraco/backoffice/uploader/upload",
                        label: 'Drop and drop a file, or click to select',
                        maxSize: 104857600,
                        maxQueue: 1,
                        postData: {
                            "allowedExtensions": $scope.model.config.allowedExtensions,
                            "maxFileSizeMb": $scope.model.config.maxFileSizeMb,
                            "imagesOnly": $scope.imageMode,
                            "minImageWidth": $scope.model.config.minImageWidth,
                            "maxImageWidth": $scope.model.config.maxImageWidth,
                            "minImageHeight": $scope.model.config.minImageHeight,
                            "maxImageHeight": $scope.model.config.maxImageHeight
                        }
                    }).on("filestart.upload", function (e, file) {
                        $("#" + $scope.uniqueId + " .radial-progress").attr("data-progress", 0);

                        $scope.uploadInProgress = true;
                    }).on("fileprogress.upload", function (e, file, percent) {
                        $("#" + $scope.uniqueId + " .radial-progress").attr("data-progress", percent);
                    }).on("filecomplete.upload", function (e, file, response) {
                        $scope.processResponse(null, response);
                    }).on("fileerror.upload", function (e, file, response) {
                        $scope.processResponse("The specified file either has an invalid extensions, exceeds the maximum allowed size, or does not meet dimension constraints");
                    });
                }, 1000);
            });

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Uploader/formstone/upload.css");
        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Uploader/N3O.Umbraco.Uploader.css");
        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Uploader/radial-progress.css");
    });
