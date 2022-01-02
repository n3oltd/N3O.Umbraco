angular.module("umbraco").controller("N3O.Umbraco.Cropper",
    function ($scope, assetsService, $timeout) {
        $scope.copytoClipboard = function (text) {
            var $temp = $("<input>");
            $("body").append($temp);
            $temp.val(text).select();
            document.execCommand("copy");
            $temp.remove();
        };

        $scope.createCropTool = function (cropIndex, hidden, restoreCropData) {
            var cropDefinition = $scope.model.config.cropDefinitions[cropIndex];

            $scope.cropperLoading = true;
            $scope.cropIndex = null;

            var $cropToolWrapper = $("#" + $scope.uniqueId + " .crop-tool-wrapper");

            $cropToolWrapper.find(".crop-tool").remove();

            var cropToolId = $scope.makeId();
            var $cropTool = $("<div />", {id: cropToolId, class: "crop-tool " + (hidden ? "hidden" : "")});
            $cropTool.prependTo($cropToolWrapper);

            var imgId = $scope.makeId();
            var $img = $("<img />", {id: imgId, src: $scope.model.value.src})
            $img.appendTo($cropTool);

            var cropper = new Cropper($img[0], {
                aspectRatio: (cropDefinition.width / cropDefinition.height).toFixed(3),
                autoCrop: true,
                strict: true,
                guides: false,
                highlight: true,
                dragCrop: true,
                movable: true,
                resizable: true,
                zoomable: false,
                viewMode: 2,
                crop: function (cropData) {
                    if ($scope.cropperLoading && restoreCropData) {
                        return;
                    }

                    $scope.model.value.crops[cropIndex] = this.cropper.getData(true);
                    $scope.model.value.cropBoxes[cropIndex] = this.cropper.getCropBoxData();

                    var cropDefinition = $scope.model.config.cropDefinitions[cropIndex];

                    $scope.requiredCropWidth = cropDefinition.width;
                    $scope.requiredCropHeight = cropDefinition.height;

                    $scope.currentCropHeight = $scope.model.value.crops[cropIndex].height;
                    $scope.currentCropWidth = $scope.model.value.crops[cropIndex].width;


                    $scope.showCropSize = $scope.currentCropHeight < $scope.requiredCropHeight || $scope.currentCropWidth < $scope.requiredCropWidth;

                    if (!$scope.cropperLoading) {
                        $scope.$apply();
                    }
                },
                ready: function () {
                    $scope.cropperLoading = false;

                    $scope.cropIndex = cropIndex;

                    if (restoreCropData && $scope.model.value.crops[cropIndex]) {
                        this.cropper.setCropBoxData($scope.model.value.cropBoxes[cropIndex]);
                    }

                    if ((cropIndex - 1) >= 0 && $scope.model.value.crops[cropIndex - 1] === null) {
                        $scope.createCropTool(cropIndex - 1, (cropIndex - 1) !== 0, false);
                    }

                    $scope.$apply();
                }
            });
        };

        $scope.getCropButtonClass = function (cropIndex) {
            return "button cursor " + ($scope.cropIndex !== null && $scope.cropIndex === cropIndex ? "selected" : "not-selected");
        };

        $scope.getCropLabel = function (cropIndex) {
            return $scope.model.config.cropDefinitions[cropIndex].label;
        };

        $scope.loadMediaById = function () {
            if (!$scope.mediaId || $scope.mediaId.length !== 12) {
                return;
            }

            $.ajax({
                type: "POST",
                url: "/Umbraco/backoffice/Api/Cropper/GetMediaById",
                data: {mediaId: $scope.mediaId},
                error: function () { $scope.processResponse(false); },
                success: function (json) { $scope.processResponse(true, json); },
                converters: {
                    "text json": function (result) {
                        return result;
                    }
                },
            });
        };

        $scope.makeId = function () {
            var text = "";
            var possible = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            for (var i = 0; i < 10; i++) {
                text += possible.charAt(Math.floor(Math.random() * possible.length));
            }

            return text;
        };

        $scope.processResponse = function (success, json) {
            if (success) {
                var response = JSON.parse(json);
                
                $scope.model.value = {
                    src: response.urlPath,
                    mediaId: response.mediaId,
                    filename: response.filename,
                    width: response.width,
                    height: response.height,
                    crops: new Array($scope.model.config.cropDefinitions.length),
                    cropBoxes: new Array($scope.model.config.cropDefinitions.length)
                };

                for (var i = 0; i < $scope.model.value.crops.length; i++) {
                    $scope.model.value.crops[i] = null;
                    $scope.model.value.cropBoxes[i] = null;
                }

                $scope.createCropTool($scope.model.value.crops.length - 1, false, false);
            } else {
                $scope.errorMessage = "The specified file is either not a valid image, exceeds the maximum allowed image size, or does not meet dimension constraints";
            }

            $scope.uploadInProgress = false;
        };

        $scope.selectCrop = function (cropIndex) {
            $scope.createCropTool(cropIndex, false, true);
        }

        $scope.startOver = function (showConfirmPrompt) {
            if (showConfirmPrompt && !confirm("Are you sure?")) {
                return;
            }

            var $cropToolWrapper = $("#" + $scope.uniqueId + " .crop-tool-wrapper");

            $cropToolWrapper.find(".crop-tool").remove();

            $scope.model.value = null;
            $scope.errorMessage = null;
        };

        $scope.uploadInProgress = false;
        $scope.errorMessage = null;

        $scope.minimumImageWidth = 0;
        $scope.minimumImageHeight = 0;

        $scope.showCropSize = false;
        $scope.currentCropHeight = 0;
        $scope.currentCropWidth = 0;
        $scope.requiredCropWidth = 0;
        $scope.requiredCropHeight = 0;

        for (var i = 0; i < $scope.model.config.cropDefinitions.length; i++) {
            var cropDefinition = $scope.model.config.cropDefinitions[i];

            if (cropDefinition.width) {
                $scope.minimumImageWidth = Math.max(cropDefinition.width, $scope.minimumImageWidth);
            }

            if (cropDefinition.height) {
                $scope.minimumImageHeight = Math.max(cropDefinition.height, $scope.minimumImageHeight);
            }
        }

        $scope.uniqueId = $scope.makeId();

        assetsService
            .load([
                "~/App_Plugins/N3O.Umbraco.Cropper/cropperjs/cropper.min.js",
                "~/App_Plugins/N3O.Umbraco.Cropper/formstone/core.js",
                "~/App_Plugins/N3O.Umbraco.Cropper/formstone/upload.js"
            ])
            .then(function () {
                $timeout(function () {
                    window.setTimeout(function () {
                        if ($scope.model.value) {
                            if ($scope.model.value.crops.length === $scope.model.config.cropDefinitions.length) {
                                $scope.selectCrop(0);
                            } else {
                                $scope.model.value.crops = new Array($scope.model.config.cropDefinitions.length);
                                $scope.model.value.cropBoxes = new Array($scope.model.config.cropDefinitions.length);

                                for (var i = 0; i < $scope.model.value.crops.length; i++) {
                                    $scope.model.value.crops[i] = null;
                                    $scope.model.value.cropBoxes[i] = null;
                                }

                                $scope.selectCrop(0);
                            }
                        }

                        $("#" + $scope.uniqueId + " .upload").upload({
                            action: "/Umbraco/backoffice/cropper/upload",
                            label: "Drop an image, or click to select. Min. size " + $scope.minimumImageWidth + " x " + $scope.minimumImageHeight + ".",
                            maxSize: 104857600,
                            maxQueue: 1,
                            postData: {
                                "minWidth": $scope.minimumImageWidth,
                                "minHeight": $scope.minimumImageHeight,
                            }
                        }).on("filestart.upload", function (e, file) {
                            $("#" + $scope.uniqueId + " .radial-progress").attr("data-progress", 0);

                            $scope.uploadInProgress = true;
                        }).on("fileprogress.upload", function (e, file, percent) {
                            $("#" + $scope.uniqueId + " .radial-progress").attr("data-progress", percent);
                        }).on("filecomplete.upload", function (e, file, response) {
                            $scope.processResponse(true, response);
                        }).on("fileerror.upload", function (e, file, response) {
                            $scope.processResponse(false);
                        });
                    }, 1000);
                })
            });

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Cropper/cropperjs/cropper.min.css");
        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Cropper/formstone/upload.css");
        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Cropper/N3O.Umbraco.Cropper.css");
        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Cropper/radial-progress.css");
    });

angular.module("umbraco").controller("N3O.Umbraco.Cropper.CropDefinitions",
    function ($scope) {
        $scope.addCropDefinition = function () {
            $scope.model.value.push({
                label: "",
                alias: "",
                width: null,
                height: null,
                filters: null
            });
        };

        $scope.deleteCropDefinition = function (crop) {
            $scope.model.value.splice($scope.model.value.indexOf(crop), 1);
        };

        if (!$scope.model.value) {
            $scope.model.value = [];

            $scope.addCropDefinition();
        }
    });
