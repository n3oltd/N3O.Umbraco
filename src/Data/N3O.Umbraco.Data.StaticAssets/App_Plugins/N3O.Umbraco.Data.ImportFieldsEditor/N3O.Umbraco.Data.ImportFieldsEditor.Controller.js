angular.module("umbraco").controller("N3O.Umbraco.Data.ImportFieldsEditor",
    function ($scope, assetsService) {
        $scope.uploadResource = async function (reference, index) {
            var uploadInput = document.getElementById("fileInput_" + index);
            const storageToken = await getStorageToken(uploadInput);

            let req = {
                file: storageToken
            };

            let res = await fetch(`/umbraco/backoffice/api/Imports/queued/${reference}/files`, {
                method: "POST",
                headers: {
                    "Accept": "*/*",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(req)
            });

            if (res.status === 200) {
                let textInput = document.getElementById("input_" + index);
                
                textInput.value = uploadInput.files[0].name;
            } else {
                alert("Failed to upload specified file, please contact support for assistance");
            }
        }

        async function getStorageToken(uploadInput) {
            const data = new FormData();
            data.append("file", uploadInput.files[0]);

            var res = await fetch("/umbraco/api/Storage/tempUpload", {
                method: "POST",
                body: data
            });

            return await res.json();
        }

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Data.ImportFieldsEditor/N3O.Umbraco.Data.ImportFieldsEditor.css");
    });