angular.module("umbraco")
    .controller("N3O.Umbraco.Data.Import", function ($scope, editorState, contentResource) {
        $scope.getTemplate = async function() {
            const content = await contentResource.getById(editorState.current.id);
            const csvTemplate = await fetch(`/umbraco/backoffice/api/Import/template/${content.key}`);
            const blob = await csvTemplate.blob();
            
            const header = csvTemplate.headers.get('Content-Disposition');
            const parts = header.split(';');
            const filename = parts[1].split('=')[1].replaceAll('"', '');
            
            const newBlob = new Blob([blob]);
            const blobUrl = window.URL.createObjectURL(newBlob);
            const link = document.createElement('a');
            link.href = blobUrl;
            link.setAttribute('download', filename);
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
            window.URL.revokeObjectURL(blobUrl);
        };

        $scope.importFile = async function() {
            const content = await contentResource.getById(editorState.current.id);
            const csvStorageToken = await getStorageToken('#csvFile');
            const zipStorageToken = await getStorageToken('#zipFile');
            
            var req = {
                datePattern: $scope.dateFormat.id,
                csvFile: csvStorageToken,
                zipFile: zipStorageToken
            };
            
            await fetch(`/umbraco/backoffice/api/Import/queue/${content.key}`, {
                method: 'POST',
                headers: {
                    'accept': '*/*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(req)
            });
        };

        fetch('/umbraco/backoffice/api/Import/lookups/datePatterns', {
            headers: {
                'accept': 'application/json'
            }
        })
        .then(res => res.json())
        .then(res => {
            $scope.dateFormats = res;
            $scope.dateFormat = res[0];
        });
        
        async function getStorageToken(selector) {
            const input = document.querySelector(selector);
            
            if (input.files.length === 0) {
                return null;
            }
            
            const data = new FormData();
            data.append('file', input.files[0]);
            
            var res = await fetch('/umbraco/api/Storage/upload', {
                method: 'POST',
                body: data
            });
            return  await res.json(); 
        }
    });