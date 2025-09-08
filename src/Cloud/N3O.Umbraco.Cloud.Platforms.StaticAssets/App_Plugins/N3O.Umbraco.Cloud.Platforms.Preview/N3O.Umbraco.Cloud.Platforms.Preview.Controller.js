angular.module("umbraco").controller("N3O.Umbraco.Cloud.Platforms.Preview",
    async function ($scope, editorState, contentEditingHelper) {
        let activeContainerId = "platformsPreviewContainer";
        let inactiveContainerId = "platformsPreviewContainer2";

        await loadDonationFormPreviewAsync(editorState, contentEditingHelper)

        window.setInterval(async function() {
            await loadDonationFormPreviewAsync(editorState, contentEditingHelper)
        }, 10000);

        async function loadDonationFormPreviewAsync(editorState, contentEditingHelper) {
            let currentVariant = editorState.current.variants.find(v => v.active);

            let properties = contentEditingHelper.getAllProps(currentVariant);
            let apiReq = getApiReq(properties);
            populateMetadata(apiReq, editorState.current)

            let apiRes = await fetch(`/umbraco/backoffice/api/platformsBackOffice/previewHtml/${editorState.current.contentTypeAlias}`, {
                method: "POST",
                headers: {
                    "accept": "application/json",
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(apiReq)
            });

            let res = await apiRes.json();

            let inactiveContainer = document.getElementById(inactiveContainerId);
            inactiveContainer.innerHTML = "";

            var iframe = document.createElement("iframe");
            iframe.style.width = "100%";
            iframe.style.aspectRatio = "16 / 9";
            iframe.style.border = "0";
            iframe.style.transform = "scale(0.9)";
            iframe.style.transformOrigin = "0 0";

            inactiveContainer.appendChild(iframe);

            var doc = iframe.contentWindow.document;
            doc.open();
            doc.write(res.html);
            doc.close();

            const script = doc.createElement("script");
            script.src = "https://cdn.n3o.cloud/connect-6e/platforms/js/platforms.js";
            script.type = "module";

            doc.body.appendChild(script);

            setTimeout(() => {
                document.getElementById(activeContainerId).style.display = "none";
                document.getElementById(inactiveContainerId).style.display = "block";

                [activeContainerId, inactiveContainerId] = [inactiveContainerId, activeContainerId];
            }, 1000);
        }

        function populateMetadata(apiReq, content) {
            let currentVariant = editorState.current.variants.find(v => v.active);

            apiReq["name"] = currentVariant.name;
            apiReq["key"] = content.key;
            apiReq["parentId"] = content.parentId;
        }

        function getApiReq(poperties) {
            var req = {};

            poperties.forEach(property => {
                req[property.alias] = property.value
            });

            return req;
        }
    });
