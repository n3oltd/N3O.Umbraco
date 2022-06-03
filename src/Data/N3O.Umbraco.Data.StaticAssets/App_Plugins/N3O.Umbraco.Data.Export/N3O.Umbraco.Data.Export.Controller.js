angular.module("umbraco")
    .controller("N3O.Umbraco.Data.Export", function ($scope, editorState, contentResource) {
        $scope.processing = false;
        $scope.contentType = null;
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
        })();
        
        $scope.export = async function () {
            $scope.processing = true;
            $scope.errorMessage = null;
            
            if (!$scope.contentType) {
                $scope.errorMessage = "Please select a content type";
                $scope.processing = false;

                return;
            }

            let selectedPropertyAliases = $scope.exportableProperties.filter(x => x.selected).map(x => x.alias);
            
            if ($scope.exportableProperties.length && !selectedPropertyAliases.length) {
                $scope.errorMessage = "At least one property must be selected";
                $scope.processing = false;
                
                return;
            }

            let req = {
                properties: selectedPropertyAliases,
                includeUnpublished: $scope.includeUnpublished,
                format: $scope.format
            };
            
            var res = await fetch(`/umbraco/backoffice/api/Exports/export/${$scope.content.key}/${$scope.contentType.alias}`, {
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json"
                },
                method: "POST",
                body: JSON.stringify(req)
            });

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
        }

        $scope.selectAll = function () {
            for (let property of $scope.exportableProperties) {
                property.selected = true;
            }
        }
        
        $scope.clearSelection = function () {
            for (let property of $scope.exportableProperties) {
                property.selected = false;
            }
        }

        async function getContentTypes(contentId) {
            const getContentType = await fetch(`/umbraco/api/ContentTypes/${contentId}/descendants`, {
                headers: {
                    "Accept": "application/json"
                }
            });
            
            return await getContentType.json();
        }
    });