angular.module("umbraco").controller("N3O.Umbraco.Data.ImportFieldsEditor",
    function ($scope, assetsService) {
        $scope.uploadResource = async function (reference) {

            const zipFile = document.getElementById("zipFile");
            if (zipFile.value && zipFile.value.split(".")[1].toLowerCase() != "zip") {
                alert("The selected file is not a valid ZIP file");

                return;
            }

            const zipStorageToken = await getStorageToken(zipFile);

            let req = {
                zipFile: zipStorageToken
            };

            let result = await fetch(`/umbraco/backoffice/api/Imports/imports/${reference}/files`, {
                method: "POST",
                headers: {
                    "Accept": "*/*",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(req)
            });

            if (result.status === 200) {
                alert("Successfully Uploaded");
            } else {
                alert("Error Uploading");
            }
        }

        async function getStorageToken(input) {
            if (input.files.length === 0) {
                return null;
            }

            const data = new FormData();
            data.append("file", input.files[0]);

            var res = await fetch("/umbraco/api/Storage/tempUpload", {
                method: "POST",
                body: data
            });

            return await res.json();
        }


        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Data.ImportFieldsEditor/N3O.Umbraco.Data.ImportFieldsEditor.css");
    });