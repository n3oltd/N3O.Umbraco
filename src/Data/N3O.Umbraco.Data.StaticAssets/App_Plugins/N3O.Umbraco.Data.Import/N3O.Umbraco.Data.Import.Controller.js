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
            const input = document.querySelector('#importFile')
            const data = new FormData()
            data.append('file', input.files[0])

            await fetch(`/umbraco/backoffice/api/Import/queue/${content.key}`, {
                method: 'POST',
                body: data
            });
        };
    });