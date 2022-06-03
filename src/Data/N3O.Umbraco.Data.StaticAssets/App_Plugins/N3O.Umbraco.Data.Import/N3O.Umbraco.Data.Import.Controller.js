angular.module("umbraco")
    .controller("N3O.Umbraco.Data.Import", function ($scope, editorState, contentResource) {
        initialize();
        
        $scope.getTemplate = async function() {
            const content = await contentResource.getById(editorState.current.id);
            
            const contentType=await getContentType(content.key);
            const csvTemplate = await fetch(`/umbraco/backoffice/api/Imports/template/${content.key}/${contentType}`);
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
            const contentType= await getContentType(content.key);
            
            const csvFile = document.getElementById("csvFile");
            const zipFile = document.getElementById("zipFile");
            
            var csvExtension = csvFile.value.split('.')[1];
            var zipExtension = zipFile.value.split('.')[1];
            
            if(csvFile.files.length==0 || csvExtension!="csv"){
                errorImporting("CSV File is Invalid.");
                return;
            }
            
            if(zipExtension!=null && zipExtension!="zip"){
                errorImporting("Only zip files can be uploaded for Image References.");
                return;
            }
            
            $scope.showButton="processingButton";
            $scope.$apply();
            
            const csvStorageToken = await getStorageToken(csvFile);
            const zipStorageToken = await getStorageToken(zipFile);
            
            var req = {
                datePattern: $scope.dateFormat.id,
                csvFile: csvStorageToken,
                zipFile: zipStorageToken
            };
            
            var result = await fetch(`/umbraco/backoffice/api/Imports/queue/${content.key}/${contentType}`, {
                method: 'POST',
                headers: {
                    'accept': '*/*',
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(req)
            });

            if(result.status!=200){
                result= await result.json();
                errorImporting(result);
                return;
            }
            $scope.showWindow="importSuccess";
            $scope.$apply();
            
        };

        fetch('/umbraco/backoffice/api/Imports/lookups/datePatterns', {
            headers: {
                'accept': 'application/json'
            }
        })
        .then(res => res.json())
        .then(res => {
            $scope.dateFormats = res;
            $scope.dateFormat = res[0];
        });
        
        
        $scope.failedTryAgain= function(){
            $scope.csvNotAttached=false;
            $scope.showButton="importButton";
            $scope.showWindow="importForm";
            $scope.$apply();
        };
        
        function errorImporting(message){
            $scope.showWindow="importFailed";
            if(!Array.isArray(message)){
                $scope.errorMessage=[message];
            }
            else{
                $scope.errorMessage=message;
            }
            
            $scope.$apply();
                    
        }
        

        
        function initialize(){

        $scope.showWindow="importForm";
        $scope.showButton="importButton";
        
        }
        
        
        
        async function getStorageToken(selector) {
            
            if (selector.files.length === 0) {
                return null;
            }
            
            const data = new FormData();
            data.append('file', selector.files[0]);
            
            var res = await fetch('/umbraco/api/Storage/tempUpload', {
                method: 'POST',
                body: data
            });
            
            return  await res.json(); 
        }
        
        async function getContentType(contentKey) {
            var getContentType = await fetch(`/umbraco/api/ContentTypes/${contentKey}/descendants`);
            var bodyContentType = await getContentType.json();
            
            
            function check(response){
                for(i=0;i<response.length;i++){
                    var temp=response[i]['alias'];
                    if(temp.includes('Beneficiary')){
                        return temp;
                    }
                }
            }
            return check(bodyContentType);
        }
    });