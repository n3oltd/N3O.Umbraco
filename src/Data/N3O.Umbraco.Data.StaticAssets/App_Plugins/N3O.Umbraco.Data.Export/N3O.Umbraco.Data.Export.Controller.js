angular.module("umbraco")
    .controller("N3O.Umbraco.Data.Export", function ($scope, editorState, contentResource, assetsService) {
        $scope.processing = false;
        $scope.contentType = null;
        $scope.errorMessage = null;
        $scope.includeUnpublished = false;
        $scope.format = "excel";

        $scope.refreshProperties = function() {
            fetch(`/umbraco/backoffice/api/Exports/exportableProperties/${$scope.contentType.alias}`, {
                headers: {
                    "Accept": "application/json",
                }
            })
            .then(res => res.json())
            .then(res => {
                for (let property of res) {
                    property.selected = false;
                }

                $scope.exportableProperties = res;
            });
        };

        (async () => {
            $scope.content = await contentResource.getById(editorState.current.id);
            $scope.contentTypes = await getContentTypes($scope.content.key);

            fetch("/umbraco/backoffice/api/Exports/lookups/contentMetadata", {
                headers: {
                    "Accept": "application/json"
                }
            })
            .then(res => res.json())
            .then(res => {
                for (let metadata of res) {
                    metadata.selected = metadata.autoSelected;
                }
                
                res.sort((a, b) => a.displayOrder - b.displayOrder);
                
                $scope.metadatas = res;
            });
        })();
        
        $scope.export = async function () {
            $scope.processing = true;
            $scope.errorMessage = null;
            
            if (!$scope.contentType) {
                processingError("Please select a content type");

                return;
            }

            let selectedMetadataIds = $scope.metadatas.filter(x => x.selected).map(x => x.id);
            let selectedPropertyAliases = $scope.exportableProperties.filter(x => x.selected).map(x => x.alias);
            
            if (!selectedPropertyAliases.length && !selectedMetadataIds.length) {
                processingError("At least one property or metadata field must be selected");
                
                return;
            }

            let req = {
                format: $scope.format,
                includeUnpublished: $scope.includeUnpublished,
                metadata: selectedMetadataIds,
                properties: selectedPropertyAliases
            };
            
            let res = await fetch(`/umbraco/backoffice/api/Exports/export/${$scope.content.key}/${$scope.contentType.alias}`, {
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                method: "POST",
                body: JSON.stringify(req)
            });
            
            if (res.status !== 200) {
                processingError(await res.json());

                return;
            }

            const blob = await res.blob();
            const header = res.headers.get("Content-Disposition");
            const parts = header.split(";");
            const filename = parts[1].split("=")[1].replaceAll('"', '');
            const newBlob = new Blob([blob]);
            const blobUrl = window.URL.createObjectURL(newBlob);
            const link = document.createElement("a");
            link.href = blobUrl;
            link.setAttribute("download", filename);
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
            window.URL.revokeObjectURL(blobUrl);

            $scope.processing = false;
             
            $scope.$digest();
        }

        $scope.selectAllMetadatas = function () {
            for (let metadata of $scope.metadatas) {
                metadata.selected = true;
            }
        }
        
        $scope.selectAllProperties = function () {
            for (let property of $scope.exportableProperties) {
                property.selected = true;
            }
        }

        $scope.clearSelectedMetadatas = function () {
            for (let metadata of $scope.metadatas) {
                metadata.selected = false;
            }
        }
        
        $scope.clearSelectedProperties = function () {
            for (let property of $scope.exportableProperties) {
                property.selected = false;
            }
        }
        
        function processingError(message) {
            $scope.processing = false;
            $scope.errorMessage = message;
        }

        async function getContentTypes(contentId) {
            const getContentType = await fetch(`/umbraco/api/ContentTypes/${contentId}/relations?type=descendant`, {
                headers: {
                    "Accept": "application/json"
                }
            });
            
            return await getContentType.json();
        }

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Data.Export/N3O.Umbraco.Data.Export.css");
    });