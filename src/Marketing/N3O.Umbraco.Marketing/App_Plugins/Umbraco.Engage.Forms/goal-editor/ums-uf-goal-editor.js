angular.module("engage").component("umsUfGoalEditor", {
    templateUrl: "/App_Plugins/Umbraco.Engage.Forms/goal-editor/ums-uf-goal-editor.html",
    bindings: {
        config: '<',
        goalId: '@',
    },
    controller: ["formPickerResource", function umsUfGoalEditorCtrl(formPickerResource) {
        var $ctrl = this;

        $ctrl.MODE = {
            ALL_FORMS: 0,
            SPECIFIC_FORMS: 1,
        };

        $ctrl.isLoading = true;

        $ctrl.forms = []; // Array of { id: ..., name: ... }

        $ctrl.formNameFilter = "";

        $ctrl.selectedForms = {}; // form.id => true

        $ctrl.$onInit = function $onInit() {            
            $ctrl.setConfigDefaults($ctrl.config);
            $ctrl.updateSelectedForms();          

            $ctrl.getForms().then(function (forms) {
                $ctrl.forms = forms;
            }).finally(function () {
                $ctrl.isLoading = false;
            });
        }

        $ctrl.setConfigDefaults = function setConfigDefaults(config) {
            if (!Array.isArray(config.formIds)) {
                config.formIds = [];
            }

            if (config.mode == null) {
                config.mode = $ctrl.MODE.ALL_FORMS;
            }
        }

        $ctrl.getForms = function getForms() {
            // Note: we HAVE TO PASS `null` here and cannot leave it empty or pass `undefined` then the Content-Type header is ommitted
            // and the Umbraco Forms controller returns a 415 Unsupported Media Type.
            return formPickerResource.getFormsForPicker(null);
        }

        $ctrl.toggleSelect = function toggleSelect(formId) {
            var idx = $ctrl.config.formIds.indexOf(formId);
            if (idx > -1) {
                $ctrl.config.formIds.splice(idx, 1);
            } else {
                $ctrl.config.formIds.push(formId);
            }

            $ctrl.updateSelectedForms();
        }

        $ctrl.updateSelectedForms = function updateSelectedForms() {
            for (var i = 0; i < $ctrl.config.formIds.length; i++) {
                var formId = $ctrl.config.formIds[i];
                $ctrl.selectedForms[formId] = true;
            }  
        }
    }]
});
