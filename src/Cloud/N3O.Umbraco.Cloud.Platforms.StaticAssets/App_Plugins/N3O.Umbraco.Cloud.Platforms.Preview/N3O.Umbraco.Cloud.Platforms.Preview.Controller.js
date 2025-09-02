document.addEventListener('DOMContentLoaded', async () => {
    let apiRes = await fetch(`/umbraco/backoffice/api/cloudBackOffice/subscription/code`);
    let subscriptionCode = await apiRes.text();

    const node = document.createElement('script');
    node.src = `https://cdn.n3o.cloud/connect-${subscriptionCode}/platforms-js/platforms.js`;
    node.type = 'text/javascript';
    node.async = false;

    document.getElementsByTagName('head')[0].appendChild(node);
});

angular.module("umbraco").controller("N3O.Umbraco.Cloud.Platforms.Preview",
    function ($scope, editorState, contentResource) {
        window.setTimeout(function() {
            contentResource.getById(editorState.current.id)
                .then(async function (content) {
                    let apiReq = contentEditingHelper.getAllProps(content);
                    
                    console.log(allProperties);

                    let apiRes = await fetch(`/umbraco/backoffice/api/cloudBackOffice/previewHtml/${editorState.current.contentTypeAlias}`, {
                        method: "POST",
                        headers: {
                            "Accept": "*/*",
                            "Content-Type": "text/html"
                        },
                        body: JSON.stringify(apiReq)
                    });

                    document.getElementById('platformsPreviewContainer').innerHTML = await apiRes.text();
                });
        });
    }, 4000);
