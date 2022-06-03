angular.module("umbraco")
    .controller("N3O.Umbraco.Data.Export", function ($scope, editorState, contentResource) {
			
	
	$scope.showButton="exportButton";
	
	(async () => {
		
	$scope.includeUnpublished=false;
    const content = await contentResource.getById(editorState.current.id);
	const contentType= await getContentType(content.key);
	$scope.exportableProperties=await fetch(`/umbraco/backoffice/api/Exports/exportableProperties/${content.key}/${contentType}`, {
		headers: {
			'accept': 'application/json'
		}
	})
	.then(res => res.json());
	$scope.propertyTitles=[];
	for(i=0;i<$scope.exportableProperties.properties.length;i++){
		$scope.propertyTitles[i]=$scope.exportableProperties.properties[i].columnTitle.replace("Basic Info:","");
	}
	})();
	
	
	$scope.exportFile= async function(){
		
		var selectedProperties=[];
		$scope.exportableProperties.properties.forEach(function(property) {
			if (property.selected) {
			  selectedProperties.push(property.alias);
			}
	    });
		
		
		if($scope.fileFormat==null || selectedProperties.length==0){
			if($scope.fileFormat==null){
				$scope.formatNotSelected=true;
			}
			if(selectedProperties.length==0){
				$scope.propertyNotSelected=true;
			}
			$scope.$apply();
			return;
		}
		
		$scope.showButton="processingButton";
		
		const content = await contentResource.getById(editorState.current.id);
		const contentType= await getContentType(content.key);
		
		var req = {
                    properties: selectedProperties,
                    includeUnpublished: $scope.includeUnpublished,
                    format: $scope.fileFormat
                };
		var csvExport= await fetch(`/umbraco/backoffice/api/Exports/export/${content.key}/${contentType}`,{
			headers: {
				'accept': '*/*',
                'Content-Type': 'application/json'
			},
			method: "POST",
			body:JSON.stringify(req)
		});
		
		const blob = await csvExport.blob();
		const header = csvExport.headers.get('Content-Disposition');
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
		
		//Clear All Parameters after Exporting
		reinitialize();
		
	}
	
	//select All Properties
	$scope.selectAllProperties= function(){
		$scope.exportableProperties.properties.forEach(function(property) {
			property.selected=true;
	    });
	}
	
	//Unselect All Properties
	$scope.clearAllProperties= function(){
		$scope.exportableProperties.properties.forEach(function(property) {
			property.selected=false;
	    });
	}
	
	
	function reinitialize(){
		
		$scope.clearAllProperties();
		$scope.fileFormat=null;
		$scope.includeUnpublished=false;
		$scope.showButton="exportButton";
		$scope.formatNotSelected=false;
		$scope.propertyNotSelected=false;
		$scope.$apply();
	}
	
	async function getContentType(contentKey){
		var getContentType = await fetch(`/umbraco/api/ContentTypes/${contentKey}/allowed`);
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