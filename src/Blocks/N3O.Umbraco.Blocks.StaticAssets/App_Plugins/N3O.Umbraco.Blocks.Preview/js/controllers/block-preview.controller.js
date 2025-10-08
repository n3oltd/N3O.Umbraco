angular.module('umbraco').controller('N3O.Umbraco.Blocks.Preview',
    async function ($scope, $sce, $timeout, editorState, $http, $element, umbRequestHelper) {

        let current = editorState.getCurrent();
        let active = current.variants.find(function (v) {
            return v.active;
        });

        $scope.nodeKey = current.key;

        if (active !== null) {
            if (active.language !== null) {
                $scope.language = active.language.culture;
            }
        }

        $scope.cacheBuster = Umbraco.Sys.ServerVariables.application.cacheBuster;

        $scope.documentTypeKey = current.documentType.key;
        
        $scope.blockEditorAlias = '';
        let parent = $scope.$parent;

        $scope.isGrid = false;

        $scope.contentElementAlias = $scope.block.content.contentTypeAlias;
        $scope.contentUdi = $scope.block.content.udi;
        $scope.settingsUdi = $scope.block.settingsData?.udi || '';

        while (parent.$parent) {
            if (parent.vm) {
                if (parent.vm.constructor.name == 'BlockGridController') {
                    $scope.blockEditorAlias = parent.vm.model.alias;
                    $scope.modelValue = parent.vm.model.value;
                    $scope.isGrid = true;
                    break;
                }
            }

            parent = parent.$parent;
        }

        parent = $scope.$parent;

        while (parent.$parent) {
            if (parent.vm) {
                if (parent.vm.constructor.name == 'ElementEditorContentComponentController') {
                    $scope.documentTypeKey = parent.vm.model.contentTypeKey;
                    break;
                }
            }

            parent = parent.$parent;
        }

        function getGridPreview(nodeKey,
                                blockData,
                                blockEditorAlias,
                                contentElementAlias,
                                culture,
                                documentTypeKey,
                                contentUdi,
                                settingsUdi,
                                blockIndex) {
            culture = culture || '';

            return umbRequestHelper.resourcePromise(
                $http.post(`/umbraco/backoffice/api/blockPreviewBackoffice/previewGridBlock/?nodeKey=${nodeKey}&documentTypeKey=${documentTypeKey}&contentUdi=${contentUdi}&culture=${culture}`, blockData),
                'Failed getting block preview markup'
            );
        }

        function loadPreview() {
            if ($scope.isGrid) {
                getGridPreview(
                    $scope.nodeKey,
                    $scope.modelValue,
                    $scope.blockEditorAlias,
                    $scope.contentElementAlias,
                    $scope.language,
                    $scope.documentTypeKey,
                    $scope.contentUdi,
                    $scope.settingsUdi,
                    $scope.block.index)
                    .then(function (data) {
                        const iframe = $element[0].querySelector(".block-preview-frame");
                        const doc = iframe.contentDocument || iframe.contentWindow.document;

                        doc.open();
                        doc.write(data);
                        doc.close();

                        iframe.onload = function () {
                            function resizeIframe(iframe) {
                                const body = doc.body.querySelector('.preview-content');
                                const height = body.scrollHeight;

                                iframe.style.height = height + "px";
                                iframe.style.width = "100%";
                                iframe.style.border = "none";
                                iframe.style.display = "block";

                                iframe.style.transform = `scale(0.9)`;
                            }

                            let checks = 0;

                            const interval = setInterval(() => {
                                resizeIframe(iframe);
                                if (++checks > 2) clearInterval(interval);
                            }, 100);

                            $element[0].querySelector(".preview-alert.preview-alert-info").style.display = "none";
                            $element[0].querySelector(".block-preview-frame").style.display = "block";
                        };
                    });
            }
        }

        loadPreview();

        let timeoutPromise;

        $scope.$watch('block.layout.columnSpan', function (newValue, oldValue) {
            if (newValue !== oldValue) {
                $timeout.cancel(timeoutPromise);

                timeoutPromise = $timeout(function () {
                    loadPreview(newValue, null);
                }, 500);
            }
        }, true);

        $scope.$watch('block.layout.rowSpan', function (newValue, oldValue) {
            if (newValue !== oldValue) {
                $timeout.cancel(timeoutPromise);

                timeoutPromise = $timeout(function () {
                    loadPreview(newValue, null);
                }, 500);
            }
        }, true);

        $scope.$watch('block.data', function (newValue, oldValue) {
            if (newValue !== oldValue) {
                $timeout.cancel(timeoutPromise);

                timeoutPromise = $timeout(function () {
                    loadPreview(newValue, null);
                }, 500);
            }
        }, true);

        $scope.$watch('block.settingsData', function (newValue, oldValue) {
            if (newValue !== oldValue) {
                $timeout.cancel(timeoutPromise);

                timeoutPromise = $timeout(function () {
                    loadPreview(null, newValue);
                }, 500);
            }
        }, true);

        $scope.editBlock = function ($event, block) {
            let target = $event.target;
            let blockActions = target.closest('.umb-block-grid__block--actions');
            let areaCreate = target.closest('.umb-block-grid__create-button');
            let blockCreateButton = target.closest('.umb-block-grid__block--inline-create-button');
            let blockCreateButtonLast = target.closest('.umb-block-grid__block--last-inline-create-button');
            let blockScaling = target.closest('.umb-block-grid__scale-handler') || target.closest('.--scale-mode');

            if (!blockActions && !areaCreate && !blockCreateButton && !blockCreateButtonLast && !blockScaling) {
                block.edit();
                $event.preventDefault();
                $event.stopPropagation();
                $event.stopImmediatePropagation();
                $event.cancelBubble = true;
            }
        }
    });

