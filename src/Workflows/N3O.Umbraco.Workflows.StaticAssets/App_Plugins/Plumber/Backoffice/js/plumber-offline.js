(function(){function r(e,n,t){function o(i,f){if(!n[i]){if(!e[i]){var c="function"==typeof require&&require;if(!f&&c)return c(i,!0);if(u)return u(i,!0);var a=new Error("Cannot find module '"+i+"'");throw a.code="MODULE_NOT_FOUND",a}var p=n[i]={exports:{}};e[i][0].call(p.exports,function(r){var n=e[i][1][r];return o(n||r)},p,p.exports,r,e,n,t)}return n[i].exports}for(var u="function"==typeof require&&require,i=0;i<t.length;i++)o(t[i]);return o}return r})()({1:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ComponentsModule = void 0;
const action_1 = require("./action-workflow/action");
const changedescription_1 = require("./changedescription/changedescription");
const chart_1 = require("./chart/chart");
const comments_1 = require("./comments/comments");
const config_1 = require("./config/config");
const contenttypeflow_1 = require("./contenttypeflow/contenttypeflow");
const instances_1 = require("./instances/instances");
const progress_1 = require("./progress/progress");
const submit_1 = require("./submit-workflow/submit");
const tasklist_1 = require("./tasklist/tasklist");
const tasks_1 = require("./tasks/tasks");
const workflowdiff_1 = require("./diff/workflowdiff");
const scheduling_1 = require("./scheduling/scheduling");
const applies_to_1 = require("./applies-to/applies-to");
const datepicker_1 = require("./datepicker/datepicker");
const history_1 = require("./history/history");
const grouppicker_1 = require("./grouppicker/grouppicker");
const pagesize_1 = require("./pagesize/pagesize");
const filterpicker_1 = require("./filterpicker/filterpicker");
const dayrange_1 = require("./dayrange/dayrange");
exports.ComponentsModule = angular
    .module('plumber.components', [])
    .component(action_1.WorkflowActionComponent.name, action_1.WorkflowActionComponent)
    .component(changedescription_1.ChangeDescriptionComponent.name, changedescription_1.ChangeDescriptionComponent)
    .component(chart_1.ChartComponent.name, chart_1.ChartComponent)
    .component(comments_1.CommentsComponent.name, comments_1.CommentsComponent)
    .component(config_1.ConfigComponent.name, config_1.ConfigComponent)
    .component(contenttypeflow_1.ContentTypeFlowComponent.name, contenttypeflow_1.ContentTypeFlowComponent)
    .component(instances_1.WorkflowInstancesComponent.name, instances_1.WorkflowInstancesComponent)
    .component(progress_1.ProgressComponent.name, progress_1.ProgressComponent)
    .component(submit_1.SubmitWorkflowComponent.name, submit_1.SubmitWorkflowComponent)
    .component(tasklist_1.TaskListComponent.name, tasklist_1.TaskListComponent)
    .component(tasks_1.TasksComponent.name, tasks_1.TasksComponent)
    .component(workflowdiff_1.WorkflowDiffComponent.name, workflowdiff_1.WorkflowDiffComponent)
    .component(scheduling_1.SchedulingComponent.name, scheduling_1.SchedulingComponent)
    .component(applies_to_1.AppliesToComponent.name, applies_to_1.AppliesToComponent)
    .component(datepicker_1.DatepickerComponent.name, datepicker_1.DatepickerComponent)
    .component(history_1.HistoryComponent.name, history_1.HistoryComponent)
    .component(grouppicker_1.WorkflowGroupPickerComponent.name, grouppicker_1.WorkflowGroupPickerComponent)
    .component(pagesize_1.PageSizeComponent.name, pagesize_1.PageSizeComponent)
    .component(filterpicker_1.FilterPickerComponent.name, filterpicker_1.FilterPickerComponent)
    .component(dayrange_1.DayRangeComponent.name, dayrange_1.DayRangeComponent)
    .name;

},{"./action-workflow/action":2,"./applies-to/applies-to":3,"./changedescription/changedescription":4,"./chart/chart":5,"./comments/comments":6,"./config/config":7,"./contenttypeflow/contenttypeflow":8,"./datepicker/datepicker":9,"./dayrange/dayrange":10,"./diff/workflowdiff":11,"./filterpicker/filterpicker":12,"./grouppicker/grouppicker":13,"./history/history":14,"./instances/instances":15,"./pagesize/pagesize":16,"./progress/progress":17,"./scheduling/scheduling":18,"./submit-workflow/submit":19,"./tasklist/tasklist":20,"./tasks/tasks":21}],2:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WorkflowActionComponent = void 0;
const constants_1 = require("../../js/constants");
class WorkflowAction {
    constructor($scope, $rootScope, $window, $location, navigationService, localizationService, plmbrActionsService, plmbrStateFactory) {
        this.$rootScope = $rootScope;
        this.$window = $window;
        this.$location = $location;
        this.navigationService = navigationService;
        this.buttonState = {
            approve: 'init',
            resubmit: 'init',
            reject: 'init',
            cancel: 'init',
        };
        this.comment = '';
        this.commentMaxLength = 250;
        this.currentAction = '';
        this.invalidComment = false;
        this.$onInit = () => {
            this.setButtonSuffix();
        };
        /**
         * If the app controller modifies state, we'll hear about it here, and update the view if required
         * @param {any} changes
         */
        this.$onChanges = changes => {
            if (changes.state && changes.state.currentValue.nodeId === this.state.nodeId) {
                this.state = changes.state.currentValue;
                this.setActivePermissions();
                this.setButtonSuffix();
                this.comment = null;
            }
        };
        this.setButtonSuffix = () => {
            if (!this.state || this.state.canAction) {
                this.btnSuffix = '';
                return;
            }
            this.btnSuffix = this.state.isAdmin && !this.state.canAction && !this.state.canResubmit ? this.asAdminStr : '';
            if (this.state.currentTask) {
                this.btnSuffix = this.state.currentTask.taskStatus === 2 && this.state.canResubmit ? '' : this.btnSuffix;
            }
        };
        this.actionsService = plmbrActionsService;
        this.stateFactory = plmbrStateFactory;
        this.onButtonStateChanged = $rootScope.$on(constants_1.constants.events.buttonStateChanged, (_, data) => {
            if (this.state && data.id === this.state.nodeId) {
                this.buttonState[this.currentAction] = data.state;
            }
        });
        localizationService.localizeMany(['workflow_action', 'workflow_asAdmin',])
            .then((resp) => {
            let [action, asAdmin] = resp;
            this.actionStr = action;
            this.asAdminStr = `(${asAdmin})`;
            this.setButtonSuffix();
        });
        $scope.$on('$destroy', () => {
            this.onButtonStateChanged();
            this.listen ? this.listen() : {};
        });
    }
    setActivePermissions() {
        const key = this.state.currentTask.node.new && this.state.permissions.new.length ? 'new' : this.state.permissions.active;
        this.activePermissions = this.state.permissions[key];
    }
    preview() {
        // Build the correct path so both /#/ and #/ work.
        // get the culture from mculture if nothing on state
        let query = `id=${this.state.nodeId}`;
        if (this.state.currentTask.variantCode && this.state.currentTask.variantCode !== '*') {
            query += `#?culture=${this.state.currentTask.variantCode}`;
        }
        const redirect = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath + '/preview/?' + query;
        // Chromes popup blocker will kick in if a window is opened
        // without the initial scoped request. This trick will fix that.
        const previewWindow = this.$window.open('preview/?init=true', 'umbpreview');
        if (previewWindow) {
            previewWindow.location.href = redirect;
        }
    }
    action(actionName) {
        this.currentAction = actionName;
        this.actionsService.action(this.state.currentTask, this.comment, actionName, this.state.offline);
        this.comment = null;
    }
    goToNode() {
        this.navigationService.changeSection('content');
        this.$location.path(`/content/content/edit/${this.state.nodeId}`);
        this.$rootScope.$emit(constants_1.constants.events.goToNode);
    }
    /**
     * If the instance has status === error, the error message is on the author comment
     * wrapped in square brackets. This extracts it.
     * @returns {string} c
     */
    extractErrorFromComment() {
        const c = this.state.currentTask.comment || '';
        return c.substring(c.indexOf('[') + 1, c.length - 1);
    }
}
exports.WorkflowActionComponent = {
    name: 'workflowAction',
    transclude: true,
    template:'<div class="workflow workflow-action"><div ng-if="$ctrl.state.currentTask.status === \'Errored\'" class="alert alert-error"><h4><localize key="workflow_cancelledWithError">Processing cancelled due to error</localize></h4><p ng-bind="$ctrl.extractErrorFromComment()"></p></div><div class="flex-row"><div class="flex-col-1-3"><umb-box><div class="umb-box-header"><div class="umb-box-header-title">{{ ::$ctrl.actionStr }}</div></div><umb-box-content ng-if="!$ctrl.state.canAction && !$ctrl.state.isAdmin && !$ctrl.state.canResubmit && !$ctrl.state.isChangeAuthor"><div class="alert alert-workflow mb-0"><umb-icon icon="icon-alert"></umb-icon><localize key="workflow_userCannotAction">Current user does not have permission to action the workflow.</localize></div></umb-box-content><umb-box-content ng-if="$ctrl.state.canAction || $ctrl.state.isAdmin || $ctrl.state.canResubmit || $ctrl.state.isChangeAuthor"><workflow-comments comment="$ctrl.comment" max-length="250" invalid="$ctrl.invalidComment"></workflow-comments><label class="control-label">{{ $ctrl.actionStr }} {{ $ctrl.btnSuffix }}</label><div class="workflow-action--controls"><umb-button type="button" disabled="$ctrl.invalidComment && !$ctrl.state.isAdmin" ng-if="($ctrl.state.canAction || $ctrl.state.isAdmin) && !$ctrl.state.rejected" action="$ctrl.action(\'approve\')" state="$ctrl.buttonState.approve" button-style="success" label-key="workflow_approve"></umb-button><umb-button type="button" ng-if="$ctrl.state.canResubmit" disabled="$ctrl.invalidComment && !$ctrl.state.isAdmin" action="$ctrl.action(\'resubmit\')" state="$ctrl.buttonState.resubmit" button-style="success" label-key="workflow_resubmit"></umb-button><umb-button type="button" ng-if="($ctrl.state.canAction || $ctrl.state.isAdmin) && !$ctrl.state.rejected" disabled="$ctrl.invalidComment && !$ctrl.state.isAdmin" action="$ctrl.action(\'reject\')" state="$ctrl.buttonState.reject" button-style="warning" label-key="workflow_reject"></umb-button><umb-button type="button" ng-if="$ctrl.state.isAdmin || $ctrl.state.isChangeAuthor" disabled="$ctrl.invalidComment && !$ctrl.state.isAdmin && $ctrl.state.isChangeAuthor" action="$ctrl.action(\'cancel\')" state="$ctrl.buttonState.cancel" button-style="danger" label-key="workflow_cancel"></umb-button><div ng-if="!$ctrl.state.offline"><umb-button type="button" ng-if="$ctrl.dashboard" button-style="default" state="init" action="$ctrl.goToNode()" label-key="workflow_editButton"></umb-button><umb-button type="button" button-style="default" state="init" action="$ctrl.preview()" label-key="general_preview"></umb-button></div></div></umb-box-content></umb-box></div><div class="flex-row flex-wrap-small"><workflow-change-description ng-if="$ctrl.state.currentTask" state="$ctrl.state"></workflow-change-description><div><workflow-scheduling ng-if="$ctrl.state.currentTask.scheduledDate" item="$ctrl.state.currentTask"></workflow-scheduling><workflow-task-list ng-if="$ctrl.state.currentTask.instance" instance-guid="$ctrl.state.currentTask.instance.guid" permissions="$ctrl.activePermissions"></workflow-task-list></div></div></div></div>',
    bindings: {
        state: '<',
        instance: '<',
        dashboard: '<',
    },
    controller: WorkflowAction
};

},{"../../js/constants":22}],3:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.AppliesToComponent = void 0;
class AppliesTo {
    constructor(localizationService) {
        this.localizationService = localizationService;
        this.$onInit = () => {
            this.localizationService.localizeMany(['workflow_publishOnly', 'workflow_publishAndUnpublish', 'workflow_unpublishDisabled', 'workflow_appliesTo'])
                .then(resp => {
                this.publishOnly = resp[0];
                this.publishAndUnpublish = resp[1];
                this.unpublishDisabled = resp[2];
                this.appliesTo = resp[3];
            });
        };
    }
}
const template = `
    <div ng-disabled="$ctrl.disabled" class="mt-2">
        <label class="control-label mb-0" for="appliesTo">{{ $ctrl.appliesTo }}</label>
        <div>
            <umb-radiobutton value="0"
                             name="appliesTo_0"
                             text="{{ $ctrl.publishAndUnpublish }}"
                             model="$ctrl.model">
            </umb-radiobutton>
            <umb-radiobutton value="1"
                             name="appliesTo_1"
                             text="{{ $ctrl.publishOnly }}"
                             model="$ctrl.model">
            </umb-radiobutton>
        </div>
        <small ng-if="$ctrl.disabled" class="mt-1" style="display:inline-block">
            {{ $ctrl.unpublishDisabled }}
        </small>
    </div>`;
exports.AppliesToComponent = {
    name: 'appliesTo',
    transclude: true,
    bindings: {
        model: '=',
        disabled: '<',
    },
    template: template,
    controller: AppliesTo
};

},{}],4:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ChangeDescriptionComponent = void 0;
class WorkflowChangeDescription {
    constructor($q, assetsService, editorService, overlayService, plmbrGroupsResource, localizationService) {
        this.editorService = editorService;
        this.overlayService = overlayService;
        this.plmbrGroupsResource = plmbrGroupsResource;
        this.showDiffBtn = false;
        this.$onChanges = changes => {
            if (changes.state && changes.state.currentVersion) {
                this.item = this.state.currentTask;
            }
        };
        this.viewsPath = Umbraco.Sys.ServerVariables.Plumber.viewsPath;
        const license = Umbraco.Sys.ServerVariables.Plumber.license;
        this.unlicensed = !license || license.isTrial && !license.isImpersonating;
        const promises = [
            assetsService.loadJs('/umbraco/lib/jsdiff/diff.js'),
            localizationService.localizeMany(['workflow_showDiff', 'workflow_viewAttachment', 'workflow_showDiff']),
        ];
        $q.all(promises).then(resp => {
            let strings = [];
            [this.loaded, strings] = resp;
            this.showDiffStr = strings[0];
            this.viewAttachmentBtnStr = strings[1];
            this.showDiffBtnStr = strings[2];
        });
    }
    $onInit() {
        this.item = this.state ? this.state.currentTask : this.item;
        this.showDiffBtn = !this.unlicensed && this.item.node.exists && !['cancelled', 'errored'].includes(this.item.cssStatus || '');
        this.language = Umbraco.Sys.ServerVariables.Plumber.languageCount > 1 ? this.item.variantName : Umbraco.Sys.ServerVariables.Plumber.defaultCultureName;
    }
    showDiff() {
        const diffOVerlay = {
            view: `${this.viewsPath}overlays/workflow.diff.overlay.html`,
            size: 'medium',
            guid: this.getInstanceGuid(),
            hideDescription: true,
            title: `${this.showDiffStr}: ${this.item.node.name}`,
            close: () => this.editorService.close()
        };
        this.editorService.open(diffOVerlay);
    }
    /**
     * Only called on active tasks, so safe to check state as it will exist
     * */
    showGroupDetails() {
        var _a, _b;
        const overlayModel = {
            view: `${Umbraco.Sys.ServerVariables.Plumber.viewsPath}overlays/groupdetail.overlay.html`,
            submitButtonLabelKey: 'workflow_editGroup',
            submitButtonStyle: 'primary',
            hideSubmitButton: !((_a = this.state) === null || _a === void 0 ? void 0 : _a.isAdmin),
            group: this.item.userGroup,
            title: (_b = this.item.userGroup) === null || _b === void 0 ? void 0 : _b.name,
            submit: model => {
                this.overlayService.close();
                this.plmbrGroupsResource.editGroup(model.group.groupId);
            },
            close: () => this.overlayService.close()
        };
        this.overlayService.open(overlayModel);
    }
    /**
     * Since the component can receive either a task or instance, need to check the correct property for the guid
     * */
    getInstanceGuid() {
        return this.item.hasOwnProperty('instanceGuid') ? this.item['instanceGuid'] : this.item['instance'].guid;
    }
}
exports.ChangeDescriptionComponent = {
    name: 'workflowChangeDescription',
    transclude: true,
    template:'<div class="workflow"><umb-box><umb-box-header title="Change description"></umb-box-header><umb-box-content><div class="flex items-center"><div class="history-item__avatar"><umb-avatar size="m" color="secondary" name="{{ ::$ctrl.item.requestedBy }}"></umb-avatar></div><div><div ng-bind="::$ctrl.item.requestedBy"></div><div class="history-item__date" ng-bind="::$ctrl.item.requestedOn"></div></div></div><div class="flex mt-2 history-item__comment"><umb-icon icon="icon-quote"></umb-icon><p ng-bind="$ctrl.item.instance.comment || $ctrl.item.comment" class="comment-text"></p></div><div class="flex mt-2"><a class="btn umb-button umb-button--xs btn-outline mr-2" ng-if="$ctrl.item.instance.attachment" href="{{ $ctrl.item.instance.attachment }}" target="_blank">{{ $ctrl.viewAttachmentBtnStr }}</a> <button class="btn umb-button umb-button--xs btn-outline" type="button" ng-if="$ctrl.showDiffBtn" aria-haspopup="true" ng-click="$ctrl.showDiff()">{{ $ctrl.showDiffBtnStr }}...</button></div></umb-box-content></umb-box><div class="flex-900 justify-between flex-wrap"><div class="umb-box umb-box--half umb-box--half__900 bg-{{ $ctrl.item.cssStatus }} flex flex-col items-center justify-center"><p class="p-box-header mb0 text-uppercase" ng-bind="$ctrl.item.statusName || $ctrl.item.status"></p><div ng-if="$ctrl.item.cssStatus === \'pendingapproval\' || $ctrl.item.cssStatus === \'rejected\'" class="umb-box-content text-center"><a role="button" ng-click="$ctrl.showGroupDetails()" style="color:currentColor">{{ $ctrl.item.userGroup.name }}</a></div></div><umb-box class="umb-box--half umb-box--half__900"><umb-box-header title-key="general_language"></umb-box-header><umb-box-content><p ng-bind="$ctrl.language"></p></umb-box-content></umb-box></div></div>',
    bindings: {
        state: '<',
        item: '<',
    },
    controller: WorkflowChangeDescription
};

},{}],5:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ChartComponent = void 0;
class WorkflowChart {
    constructor($window, dateHelper, plmbrWorkflowResource) {
        this.$window = $window;
        this.dateHelper = dateHelper;
        this.now = moment.utc().endOf('day');
        this.totalApproved = 0;
        this.totalCancelled = 0;
        this.totalErrored = 0;
        this.totalPending = 0;
        this.loaded = false;
        this.colorMap = {
            approved: '#2bc37c',
            pending: '#3544b1',
            cancelled: '#eaddd5',
            errored: '#ee5f5b',
        };
        this.$onChanges = change => {
            var _a;
            if ((_a = change.range) === null || _a === void 0 ? void 0 : _a.currentValue) {
                this.getForRange();
            }
        };
        this.emptySeries = (length) => Array.from({ length }, (_, i) => ({ x: this.earliest.clone().add(i, 'd'), y: 0 }));
        this.buildChartSeries = items => {
            this.showStatsBox = items.length > 0;
            const validNames = ['Pending', 'Approved', 'Cancelled', 'Errored'];
            const series = [];
            const seriesNames = [];
            this.earliest = this.now.clone().subtract(this.range - 1, 'd');
            const pendingItems = this.emptySeries(this.range);
            const approvedItems = this.emptySeries(this.range);
            const cancelledItems = this.emptySeries(this.range);
            const erroredItems = this.emptySeries(this.range);
            const o = {
                label: 'Pending',
                borderColor: this.colorMap.pending,
                backgroundColor: this.colorMap.pending,
                data: pendingItems,
                type: 'line',
                fill: false
            };
            this.seconds = {
                min: 0,
                max: 0,
                total: 0,
            };
            series.push(o);
            seriesNames.push('Pending');
            items.forEach(v => {
                let statusName = (this.groupId ? v.statusName : v.status).split(' ')[0]; // `Pending Approval` becomes `Pending`
                let cssStatus = v.cssStatus;
                let completedDate = moment(v.completedDate ? v.completedDate.replace('Z', '') : null);
                const createdDateKey = this.groupId ? 'requestedOn' : 'createdDate';
                let createdDate = moment(v[createdDateKey].replace('Z', ''));
                // rejected items are counted as pending
                cssStatus = cssStatus === 'rejected' ? 'pendingapproval' : cssStatus;
                statusName = statusName === 'Rejected' ? 'Pending' : statusName;
                let seconds = (v.completedDate ? completedDate : moment()).diff(createdDate, 'seconds');
                this.seconds.total += seconds;
                this.seconds.max = Math.max(seconds, this.seconds.max);
                this.seconds.min = Math.min(seconds, this.seconds.min);
                if (!seriesNames.includes(statusName) && validNames.includes(statusName)) {
                    const o = {
                        label: statusName,
                        borderColor: this.colorMap[cssStatus],
                        backgroundColor: this.colorMap[cssStatus],
                        data: cssStatus === 'approved' ? approvedItems : cssStatus === 'errored' ? erroredItems : cancelledItems
                    };
                    series.push(o);
                    seriesNames.push(statusName);
                }
                const completedIndex = pendingItems.findIndex(item => item.x.dayOfYear() === completedDate.dayOfYear());
                const createdIndex = pendingItems.findIndex(item => item.x.dayOfYear() === createdDate.dayOfYear());
                pendingItems[createdIndex !== -1 ? createdIndex : 0].y += 1;
                if (completedIndex !== -1) {
                    pendingItems[completedIndex].y -= 1;
                    if (cssStatus === 'approved') {
                        this.totalApproved += 1;
                        approvedItems[completedIndex].y += 1;
                    }
                    else if (cssStatus === 'cancelled') {
                        this.totalCancelled += 1;
                        cancelledItems[completedIndex].y += 1;
                    }
                    else if (cssStatus === 'errored') {
                        this.totalErrored += 1;
                        erroredItems[completedIndex].y += 1;
                    }
                    else if (cssStatus === 'pendingapproval') {
                        this.totalPending += 1;
                    }
                }
                else if (cssStatus === 'pendingapproval') {
                    this.totalPending += 1;
                }
            });
            // accumulate pending items
            let pendingItemsData = series.find(x => x.label === 'Pending');
            if (pendingItemsData) {
                pendingItemsData.data.forEach((_, i) => {
                    if (i > 0 && pendingItemsData) {
                        pendingItemsData.data[i].y += pendingItemsData.data[i - 1].y;
                    }
                });
            }
            this.series = series;
            this.averageSeconds = moment.duration(this.seconds.total / items.length, 'seconds');
            this.maxSeconds = moment.duration(this.seconds.max, 'seconds');
            this.minSeconds = moment.duration(this.seconds.min, 'seconds');
            // finally, init the chart
            this.drawChart();
        };
        this.getForRange = () => {
            if (this.range > 0) {
                this.totalApproved = 0;
                this.totalCancelled = 0;
                this.totalPending = 0;
                this.totalErrored = 0;
                this.seconds = {
                    min: 0,
                    max: 0,
                    total: 0,
                };
                this.loaded = false;
                // one less than range since current date is included
                // if a group id exists, the endpoint is different
                (this.groupId ?
                    this.workflowResource.getAllTasksForGroupForRange(this.groupId, this.range - 1) :
                    this.workflowResource.getAllInstancesForRange(this.range - 1))
                    .then(resp => this.buildChartSeries(resp.items));
            }
        };
        this.getActivity = filter => {
            let statusValues = [
                { key: 'Pending approval', value: '3' },
                { key: 'Approved', value: '1' },
                { key: 'Cancelled', value: '5' },
                { key: 'Errored', value: '6' }
            ];
            const f = statusValues.find(x => x.key === filter);
            const o = {
                status: []
            };
            if (f) {
                // if filtering pending, include rejected and resubmitted
                if (filter !== 'Pending approval') {
                    o.status = [f.value];
                }
                else {
                    o.status = [3, 2, 7];
                }
            }
            // if the key is NOT pending, filter by items completed inside the current range
            if (filter !== 'Pending approval') {
                let from = this.dateHelper.convertToServerStringTime(this.earliest, Umbraco.Sys.ServerVariables.application.serverTimeOffset);
                o.completedFrom = from;
            }
            this.workflowResource.setActivityFilter(o);
            this.$window.location = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath +
                '/#/workflow/history/overview';
        };
        this.drawChart = () => {
            Chart.defaults.scale.gridLines.display = false;
            Chart.defaults.scale.ticks.beginAtZero = true;
            const chart = document.getElementById('chart');
            if (!chart)
                return;
            const ctx = chart.getContext('2d');
            if (this.chart && this.chart.destroy)
                this.chart.destroy();
            this.chart = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: this.emptySeries(this.range).map(x => x.x.format('D MMM')),
                    datasets: this.series
                },
                options: {
                    scales: {
                        yAxes: [
                            {
                                ticks: {
                                    stepSize: 1,
                                    maxTicksLimit: 5
                                }
                            }
                        ],
                        xAxes: [
                            {
                                time: {
                                    stepSize: 4
                                }
                            }
                        ]
                    }
                }
            });
            this.loaded = true;
        };
        this.workflowResource = plmbrWorkflowResource;
    }
}
exports.ChartComponent = {
    name: 'workflowChart',
    transclude: true,
    template:'<div class="relative"><div ng-show="$ctrl.loaded"><ul class="chart-header"><li class="flex-li"><button type="button" ng-click="$ctrl.getActivity(\'Approved\')" class="bg-approved"><umb-icon icon="icon-info" title="Click to view history by state: Approved"></umb-icon><span ng-bind="$ctrl.totalApproved" class="jumbo"></span><span>approved</span></button></li><li class="flex-li"><button type="button" ng-click="$ctrl.getActivity(\'Cancelled\')" class="bg-cancelled"><umb-icon icon="icon-info" title="Click to view history by state: Cancelled"></umb-icon><span ng-bind="$ctrl.totalCancelled" class="jumbo"></span><span>cancelled</span></button></li><li class="flex-li"><button type="button" ng-click="$ctrl.getActivity(\'Pending approval\')" class="bg-pending"><umb-icon icon="icon-info" title="Click to view history by state: Pending"></umb-icon><span ng-bind="$ctrl.totalPending" class="jumbo"></span><span>pending</span></button></li><li class="flex-li"><button type="button" ng-click="$ctrl.getActivity(\'Errored\')" class="btn-reset bg-errored"><umb-icon icon="icon-info" title="Click to view history by state: Errored"></umb-icon><span ng-bind="$ctrl.totalErrored" class="jumbo"></span> <span>errored</span></button></li><li class="flex-li stats-box" ng-if="$ctrl.showStatsBox"><ul><li><strong><localize key="workflow_fastestApproval">Fastest approval</localize>:</strong> <span ng-bind="$ctrl.minSeconds.humanize()"></span></li><li><strong><localize key="workflow_slowestApproval">Slowest approval</localize>:</strong> <span ng-bind="$ctrl.maxSeconds.humanize()"></span></li><li><strong><localize key="workflow_averageApproval">Average approval</localize>:</strong> <span ng-bind="$ctrl.averageSeconds.humanize()"></span></li></ul></li></ul><canvas id="chart" width="400" height="100"></canvas></div><umb-load-indicator ng-if="!$ctrl.loaded"></umb-load-indicator></div>',
    controller: WorkflowChart,
    bindings: {
        groupId: '<',
        range: '<',
    }
};

},{}],6:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.CommentsComponent = void 0;
const constants_1 = require("../../js/constants");
class WorkflowComments {
    constructor(localizationService, $sce, $rootScope) {
        this.localizationService = localizationService;
        this.$sce = $sce;
        this.defaultMaxLength = 250;
        this.maxLengthStr = '';
        this.remainingStr = '';
        this.$onInit = () => {
            this.maxLength = this.maxLength || this.defaultMaxLength;
            this.localizationService.localizeMany([this.labelKey || 'workflow_addComment', this.templateKey || '', 'workflow_commentRemaining', 'workflow_commentMaxLength'])
                .then((resp) => {
                this.labelStr = resp[0];
                const template = resp[1];
                this.remainingStr = resp[2];
                this.maxLengthStr = resp[3].replace('%0%', this.maxLength.toString());
                if (!template.startsWith('[') && !template.endsWith(']')) {
                    this.comment = this.$sce.trustAsHtml(template);
                    this.limitChars(template.length);
                }
                else {
                    this.limitChars();
                }
            });
        };
        this.$onDestroy = () => {
            this.onActioned();
        };
        /**
         * Optionally provide a numeric value to set the initial counter
         * Used when setting a template as the escaped string doesn't set the model
         * until it is modified, but does have a length
         * @param length
         */
        this.limitChars = (length) => {
            var _a;
            length = length || ((_a = this.comment) === null || _a === void 0 ? void 0 : _a.length);
            if (length > this.maxLength) {
                this.info = this.maxLengthStr;
                this.comment = this.comment.substr(0, this.maxLength);
                this.invalid = true;
            }
            else {
                this.info = this.remainingStr.replace('%0%', (this.maxLength - (length || 0)).toString());
                this.invalid = !length;
            }
        };
        this.onActioned = $rootScope.$on(constants_1.constants.events.workflowActioned, () => {
            this.limitChars();
        });
    }
}
const template = `
    <div class="umb-el-wrap">
        <label class="control-label" for="workflowComment">
            {{ $ctrl.labelStr }}
            <small ng-bind="$ctrl.info"></small>
        </label>
        <div class="controls">
            <textarea
                id="workflowComment"
                ng-model="$ctrl.comment"
                ng-change="$ctrl.limitChars()"
                no-dirty-check
                umb-auto-focus
                rows="5"                     
                class="umb-property-editor umb-textarea"></textarea>
        </div>
    </div>`;
exports.CommentsComponent = {
    name: 'workflowComments',
    transclude: true,
    bindings: {
        comment: '=',
        maxLength: '<',
        invalid: '=',
        templateKey: '<',
        labelKey: '<'
    },
    template: template,
    controller: WorkflowComments
};

},{"../../js/constants":22}],7:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ConfigComponent = void 0;
const constants_1 = require("../../js/constants");
class WorkflowConfig {
    constructor($scope, $rootScope, $element, $location, editorService, plmbrGroupsResource, plmbrWorkflowResource) {
        this.$scope = $scope;
        this.$rootScope = $rootScope;
        this.$element = $element;
        this.$location = $location;
        this.editorService = editorService;
        this.sortOptions = {
            axis: 'y',
            containment: 'parent',
            distance: 10,
            opacity: 0.7,
            tolerance: 'pointer',
            scroll: true,
            zIndex: 6000,
            stop: () => { },
        };
        /**
         * */
        this.checkActiveType = () => {
            this.activeType =
                this.approvalPath.length || this.useNewNodePath ? 'content' :
                    this.contentTypeApprovalPath.length ? 'type' :
                        this.inherited.length ? 'inherited' : null;
        };
        this.workflowGroupsResource = plmbrGroupsResource;
        this.workflowResource = plmbrWorkflowResource;
        this.inherited = [];
        this.approvalPath = [];
        this.contentTypeApprovalPath = [];
        this.sortOptions.stop = () => {
            this.$scope.contentFlowForm.$setDirty();
        };
    }
    /**
     * Process the approvalPath object, then save it
     */
    save() {
        // ensure everything being saved has the correct variant and appliesTo value
        // need to make sure this doesn't break anything when the original path was inherited
        // also ensures the permission is set correctly as the sort order may have changed
        this.approvalPath.forEach((v, i) => {
            v.variant = this.variant;
            v.type = +this.appliesTo;
            v.permission = i;
        });
        this.workflowResource.saveNodeConfig(this.node.id, this.approvalPath, this.variant, +this.appliesTo)
            .then(() => {
            this.$scope.contentFlowForm.$setPristine();
            this.$element.inheritedData('$formController').$setPristine();
            this.$rootScope.$emit(constants_1.constants.events.configSaved, { id: this.node.id });
        });
    }
    /**
     * */
    openGroupOverlay() {
        const model = {
            view: Umbraco.Sys.ServerVariables.Plumber.viewsPath + '/overlays/grouppicker.overlay.html',
            size: constants_1.constants.sizes.s,
            title: 'Add workflow approval group/s',
            approvalPath: this.approvalPath,
            submit: (result) => {
                result.selection.forEach(group => {
                    this.add(group);
                });
                this.editorService.close();
            },
            close: () => this.editorService.close(),
        };
        this.editorService.open(model);
    }
    groupName(name, idx) {
        return this.workflowGroupsResource.generateNameWithStage(name, idx);
    }
    editGroup(group) {
        this.workflowGroupsResource.editGroup(group.groupId);
    }
    /**
     * */
    removeAll() {
        this.approvalPath = [];
        this.checkActiveType();
        this.save();
    }
    /**
     * Adds a stage to the approval flow
     */
    add(group) {
        this.$scope.contentFlowForm.$setDirty();
        this.approvalPath.push({
            groupName: group.name,
            nodeId: this.node.id,
            permission: this.approvalPath.length,
            groupId: group.groupId
        });
        this.checkActiveType();
    }
    /**
     * Removes a stage from the approval flow
     * @param {any} $event
     * @param {any} item
     */
    remove(group) {
        this.$scope.contentFlowForm.$setDirty();
        const idx = this.approvalPath.findIndex(x => x.groupId === group.groupId);
        this.approvalPath.splice(idx, 1);
        this.approvalPath.forEach((v, i) => v.permission = i);
        this.checkActiveType();
        if (this.approvalPath.length === 0) {
            this.save();
        }
    }
    $onInit() {
        const activeVariant = this.node.variants.find(x => x.active);
        const isCreate = this.$location.search().create === 'true';
        const variant = !isCreate && (activeVariant === null || activeVariant === void 0 ? void 0 : activeVariant.language) ? activeVariant.language.culture : this.$location.search().cculture;
        this.variant = variant || Umbraco.Sys.ServerVariables.Plumber.defaultCulture;
        this.useNewNodePath = !(activeVariant === null || activeVariant === void 0 ? void 0 : activeVariant.publishDate) && this.state.permissions.new.length > 0;
        this.approvalPath = this.state.permissions.node;
        this.contentTypeApprovalPath = this.state.permissions.contentType;
        this.inherited = this.state.permissions.inherited;
        this.newNodePath = this.state.permissions.new;
        // set the applies to checkbox to match the node-level approvals (all will have the same value)
        // if no node level, check if unpublish is required - if it is, type is publish+unpubish, else publish only
        this.appliesTo = (this.approvalPath.length ? this.approvalPath[0].type : this.state.requireUnpublish ? 0 : 1).toString();
        this.checkActiveType();
        this.path = isCreate ? '-1' : this.node.path;
    }
}
exports.ConfigComponent = {
    name: 'workflowConfig',
    transclude: true,
    template:'<div class="workflow flex-row"><div><div class="umb-box" ng-class="{\'active\' : $ctrl.activeType === \'content\'}"><umb-box-header title-key="workflow_contentApprovalFlow"><umb-badge ng-if="$ctrl.activeType === \'content\'" size="xxs"><localize key="workflow_active">ACTIVE</localize></umb-badge></umb-box-header><div class="umb-box-content"><div class="alert alert-workflow mb-0 pr-2" ng-if="$ctrl.node.id === 0"><umb-icon icon="icon-alert"></umb-icon><localize key="workflow_newNodeConfig"></localize></div><div class="alert alert-workflow mb-0 pr-2" ng-if="$ctrl.useNewNodePath"><p><localize key="workflow_newNodeFlowDescription"></localize></p><umb-node-preview ng-repeat="group in $ctrl.newNodePath track by $index" name="$ctrl.groupName(group.groupName, $index)" sortable="false" allow-remove="false" allow-edit="false"></umb-node-preview></div><ng-form name="contentFlowForm" class="mb-0 pos-relative" novalidate ng-hide="$ctrl.node.id === 0"><div class="current-flow pos-relative" ui-sortable="$ctrl.sortOptions" ng-model="$ctrl.approvalPath" ng-show="$ctrl.approvalPath.length"><umb-node-preview ng-repeat="group in $ctrl.approvalPath track by $index" name="$ctrl.groupName(group.groupName, $index)" sortable="true" allow-remove="true" allow-edit="true" on-remove="$ctrl.remove(group)" on-edit="$ctrl.editGroup(group)"></umb-node-preview></div><button type="button" class="umb-node-preview-add" ng-click="$ctrl.openGroupOverlay()"><localize key="general_add">Add</localize></button><applies-to model="$ctrl.appliesTo" disabled="!$ctrl.state.requireUnpublish"></applies-to><div class="d-flex mt-2"><button class="btn btn-primary" ng-click="$ctrl.save()" ng-disabled="!contentFlowForm.$dirty" type="button"><localize key="workflow_save">Save</localize></button> <button class="btn btn-info ml-1" ng-click="$ctrl.removeAll()" ng-disabled="!$ctrl.approvalPath.length" type="button"><localize key="workflow_removeAll">Remove all</localize></button></div></ng-form></div></div></div><div><div class="umb-box" ng-class="{ \'active\' : $ctrl.activeType === \'inherited\' }"><umb-box-header title-key="workflow_inheritedApprovalFlow"><umb-badge ng-if="$ctrl.activeType === \'inherited\'" size="xxs"><localize key="workflow_active">ACTIVE</localize></umb-badge></umb-box-header><div class="umb-box-content"><div ng-if="$ctrl.inherited.length"><p><localize key="workflow_currentPageInheritsFrom"></localize>&nbsp;<strong ng-bind="::$ctrl.inherited[0].nodeName"></strong> for <strong ng-bind="::$ctrl.inherited[0].type === 0 ? \'publish and unpublish\' : \'publish\'"></strong> workflows.</p><div class="current-flow"><div class="umb-node-preview" ng-repeat="group in $ctrl.inherited track by $index"><div class="umb-node-preview__content">{{ $ctrl.groupName(group.groupName, $index) }}</div><div class="umb-node-preview__actions"><button class="umb-node-preview__action" ng-click="$ctrl.editGroup(group)"><localize key="workflow_editGroup">Edit group</localize></button></div></div></div></div><span class="alert alert-workflow mb-0 inline-block" ng-if="!$ctrl.inherited.length"><localize key="workflow_noInheritedFlow">No inherited approval flow exists for this document</localize></span></div></div><div class="umb-box" ng-class="{\'active\' : $ctrl.activeType === \'type\'}" licensed><umb-box-header title-key="workflow_docTypeApprovalFlow"><umb-badge ng-if="$ctrl.activeType === \'type\'" size="xxs"><localize key="workflow_active">ACTIVE</localize></umb-badge></umb-box-header><div class="umb-box-content"><div ng-if="$ctrl.contentTypeApprovalPath.length"><p>This document-type has configuration for <strong ng-bind="::$ctrl.contentTypeApprovalPath[0].type === 0 ? \'publish and unpublish\' : \'publish\'"></strong> workflows.</p><div class="current-flow"><div class="umb-node-preview" ng-repeat="group in $ctrl.contentTypeApprovalPath track by $index"><div class="umb-node-preview__content">{{ $ctrl.groupName(group.groupName, $index) }}</div><div class="umb-node-preview__actions"><a class="umb-node-preview__action" ng-click="$ctrl.editGroup(group)"><localize key="workflow_editGroup">Edit group</localize></a></div></div></div></div><span class="alert alert-workflow mb-0 inline-block" ng-if="!$ctrl.contentTypeApprovalPath.length"><localize key="workflow_noDoctypeFlow">No document type flow set for</localize>&nbsp;{{ $ctrl.node.contentTypeName }}</span></div></div></div></div>',
    bindings: {
        node: '<',
        state: '<'
    },
    controller: WorkflowConfig
};

},{"../../js/constants":22}],8:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ContentTypeFlowComponent = void 0;
const constants_1 = require("../../js/constants");
class WorkflowContentTypeFlow {
    constructor(languageResource, plmbrGroupsResource, editorService) {
        this.languageResource = languageResource;
        this.editorService = editorService;
        this.init = () => {
            var _a;
            if (this.model.type) {
                this.model.type.variant = (_a = this.languages.find(x => x.isDefault)) === null || _a === void 0 ? void 0 : _a.culture;
                this.variantChanged();
                this.typeChanged();
                this.model.type.permissions.forEach((p) => {
                    if (p.condition) {
                        // reopening the overlay has the already-split condition, so don't try doing it again...
                        p.condition = typeof p.condition === 'string' ? p.condition.split(',') : p.condition;
                        p.condition.forEach(c => {
                            if (c) {
                                this.conditions.push({
                                    groupName: p.groupName,
                                    groupId: p.groupId,
                                    condition: c,
                                    variant: p.variant
                                });
                            }
                        });
                    }
                });
                // binding to radiobutton requires a string value, sad face.
                this.model.appliesTo = this.model.type.permissions[0].type.toString();
            }
            else {
                this.isAdd = true;
                this.model.appliesTo = "0";
            }
        };
        /**
         *
         */
        this.variantChanged = () => {
            let variantPermissions = this.model.type.permissions.filter(p => p.variant === this.model.type.variant);
            this.variantPermissionsCount = (variantPermissions ? variantPermissions : this.model.type.permissions.filter(p => p.variant === '*')).length;
        };
        /**
         * */
        this.typeChanged = () => {
            var _a;
            this.properties = this.model.type.properties;
            this.model.type.variant = (_a = this.languages.find(x => x.isDefault)) === null || _a === void 0 ? void 0 : _a.culture;
            this.variantChanged();
        };
        /**
         *
         */
        this.addCondition = () => this.conditions.push({
            variant: this.model.type.variant
        });
        /**
         *
         * @param {object} $event the click event
         * @param {int} index the index of the condition
         * @param {string} condition the condition value
         */
        this.removeCondition = ($event, index, condition) => {
            $event.stopPropagation();
            this.conditions.splice(index, 1);
            this.model.type.permissions.forEach((p) => {
                var _a;
                if (p.contentTypeId === this.model.type.id
                    && typeof p.condition === 'object'
                    && ((_a = p.condition) === null || _a === void 0 ? void 0 : _a.some(x => x === condition))
                    && p.variant === this.model.type.variant) {
                    p.condition.splice(p.condition.indexOf(condition), 1);
                }
            });
        };
        /**
         *
         * @param {int} groupId id of the group assigned to the condition
         * @param {string} condition rule representing the workflow stage condition
         * @param {object} oldValue the previous condition
         */
        this.setCondition = (groupId, condition, currentValue) => {
            const permission = this.model.type.permissions.find((p) => p.contentTypeId === this.model.type.id &&
                p.variant === this.model.type.variant &&
                p.groupId === groupId);
            if (permission.condition) {
                const existingIndex = permission.condition.indexOf(currentValue);
                if (existingIndex > -1) {
                    permission.condition[existingIndex] = condition;
                }
                else {
                    permission.condition.push(condition);
                }
            }
            else {
                permission.condition = [condition];
            }
        };
        /**
         *
         */
        this.add = (group) => {
            // when adding a new config, type will not exist.
            if (!this.model.type) {
                this.model.type = {
                    variant: Umbraco.Sys.ServerVariables.Plumber.defaultCulture
                };
            }
            this.model.type.permissions.push({
                contentTypeId: this.model.type.id,
                permission: this.model.type.permissions.filter((p) => p.variant === this.model.type.variant).length,
                groupId: group.groupId,
                groupName: group.name,
                variant: this.model.type.variant
            });
            this.variantChanged();
        };
        /**
         *
         * @param {object} $event click
         * @param {object} item a permissions item
         */
        this.remove = (item) => {
            this.model.type.permissions.splice(this.model.type.permissions.indexOf(item), 1);
            // also remove any conditions - can't do in the existing method as params are different.
            if (this.conditions.length > 0) {
                this.conditions = this.conditions.filter(c => c.groupId !== item.groupId);
            }
            this.variantChanged();
        };
        this.$onInit = () => {
            this.languageResource.getAll()
                .then(languages => {
                this.languages = languages;
                this.init();
            });
        };
        this.workflowGroupsResource = plmbrGroupsResource;
        this.properties = [];
        this.conditions = [];
        this.sortOptions = {
            axis: 'y',
            cursor: 'move',
            handle: '.sort-handle',
            stop: (e, ui) => {
                const permissions = this.model.type.permissions.filter(p => p.variant === this.model.type.variant);
                permissions.forEach((p, i) => {
                    p.permission = i;
                });
            }
        };
    }
    /**
 * */
    openGroupOverlay() {
        const model = {
            view: Umbraco.Sys.ServerVariables.Plumber.viewsPath + '/overlays/grouppicker.overlay.html',
            size: constants_1.constants.sizes.s,
            title: 'Add workflow approval group/s',
            approvalPath: this.model.type.permissions,
            submit: (result) => {
                result.selection.forEach(group => {
                    this.add(group);
                });
                this.editorService.close();
            },
            close: () => this.editorService.close(),
        };
        this.editorService.open(model);
    }
    groupName(name, idx) {
        return this.workflowGroupsResource.generateNameWithStage(name, idx);
    }
    editGroup(group) {
        this.workflowGroupsResource.editGroup(group.groupId);
    }
}
exports.ContentTypeFlowComponent = {
    name: 'workflowContentTypeFlow',
    transclude: true,
    template:'<umb-load-indicator ng-if="$ctrl.loading"></umb-load-indicator><umb-box ng-if="$ctrl.isAdd"><umb-box-header title-key="workflow_selectDoctypes"></umb-box-header><umb-box-content><select class="mb-0" ng-model="$ctrl.model.type" ng-options="g as g.name for g in $ctrl.model.types" ng-change="$ctrl.typeChanged()" data-element="workflow-overlay__contenttype-types"><option selected disabled>--- Select document type ---</option></select></umb-box-content></umb-box><div ng-if="$ctrl.model.type" class="workflow"><umb-box ng-if="$ctrl.languages"><umb-box-header title-key="general_language"></umb-box-header><umb-box-content><select class="mb-0" ng-disabled="$ctrl.languages.length === 1" ng-model="$ctrl.model.type.variant" ng-options="l.culture as l.name for l in $ctrl.languages" ng-change="$ctrl.variantChanged()" data-element="workflow-overlay__contenttype-languages"></select></umb-box-content></umb-box><umb-box><umb-box-header title-key="workflow_currentFlow"></umb-box-header><umb-box-content><div class="current-flow pos-relative" ui-sortable="$ctrl.sortOptions" ng-model="$ctrl.model.type.permissions" ng-show="$ctrl.model.type.permissions.length"><umb-node-preview ng-repeat="group in $ctrl.model.type.permissions track by $index" name="$ctrl.groupName(group.groupName, $index)" icon="group.icon || \'icon-users\'" sortable="true" allow-remove="true" allow-edit="true" on-remove="$ctrl.remove(group)" on-edit="$ctrl.editGroup(group)"></umb-node-preview></div><button type="button" class="umb-node-preview-add" ng-click="$ctrl.openGroupOverlay()"><localize key="general_add">Add</localize></button><applies-to model="$ctrl.model.appliesTo" disabled="!$ctrl.model.requireUnpublish"></applies-to></umb-box-content></umb-box><umb-box ng-if="!$ctrl.model.typesToAdd && $ctrl.variantPermissionsCount"><umb-box-header title="Conditional stages"></umb-box-header><umb-box-content><ul class="current-flow ml-0 mb2" ng-if="$ctrl.conditions.length"><li class="umb-node-preview" ng-repeat="c in $ctrl.conditions | permissionsByVariant: $ctrl.model.type.variant track by $index"><div class="umb-node-preview__content conditional-group"><localize key="workflow_include">Include</localize><div class="fancy-select"><select class="mb-0" ng-model="c.groupId" ng-options="p.groupId as p.groupName for p in $ctrl.model.type.permissions | permissionsByVariant: $ctrl.model.type.variant" data-element="workflow-overlay__contenttype-conditionalgroup"><option selected disabled>--- Add ---</option></select></div><localize key="workflow_when">when</localize><div class="fancy-select"><select class="mb-0" ng-model="c.condition" ng-change="$ctrl.setCondition(c.groupId, c.condition, \'{{ c.condition }}\')" ng-options="p.key as p.name for p in $ctrl.properties"><option selected disabled>--- Add ---</option></select></div><localize key="workflow_hasChanged">has changed</localize></div><div class="umb-node-preview__actions"><button type="button" class="umb-node-preview__action" ng-click="$ctrl.removeCondition($event, $index, c.condition, c.groupId)"><localize key="general_remove">Remove</localize></button></div></li></ul><button class="umb-node-preview-add" ng-click="$ctrl.addCondition()"><localize key="workflow_addCondition">Add condition</localize></button></umb-box-content></umb-box></div>',
    bindings: {
        model: '='
    },
    controller: WorkflowContentTypeFlow
};

},{"../../js/constants":22}],9:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.DatepickerComponent = void 0;
class Datepicker {
    constructor(dateHelper, userService, localizationService) {
        this.dateHelper = dateHelper;
        this.userService = userService;
        this.localizationService = localizationService;
        this.$onChanges = () => {
            if (this.date) {
                this.change(this.date);
            }
            if (this.labelKey) {
                this.localizationService.localize(this.labelKey)
                    .then(label => this.label = label);
            }
        };
        this.change = (dateStr) => {
            this.date = this.dateHelper.convertToServerStringTime(moment(dateStr), Umbraco.Sys.ServerVariables.application.serverTimeOffset);
            if (this.currentUser) {
                this.setDateFormatted();
            }
            else {
                this.userService.getCurrentUser().then((currentUser) => {
                    this.currentUser = currentUser;
                    this.setDateFormatted();
                });
            }
        };
        this.setDateFormatted = () => {
            this.dateFormatted = this.dateHelper.getLocalDate(this.date, this.currentUser.locale, "MMM Do YYYY, HH:mm");
        };
        this.clear = () => {
            this.date = null;
            this.dateFormatted = null;
        };
        var now = new Date();
        this.config = {
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            time_24hr: true,
            defaultDate: null,
            defaultHour: now.getHours(),
            defaultMinute: now.getMinutes() + 5
        };
        this.clear();
    }
}
exports.DatepickerComponent = {
    name: 'workflowDatepicker',
    bindings: {
        date: '=',
        label: '@',
        labelKey: '@'
    },
    template:'<div class="btn-group flex" style="font-size:15px"><umb-date-time-picker ng-model="$ctrl.date" options="$ctrl.config" on-change="$ctrl.change(dateStr)"><div><button type="button" ng-show="$ctrl.date" class="btn umb-button--xs" style="outline:none;">{{ $ctrl.dateFormatted }}</button> <button type="button" class="btn btn-outline-wf" ng-hide="$ctrl.date"><span>{{ $ctrl.label }}</span></button></div></umb-date-time-picker><button type="button" ng-show="$ctrl.date" ng-click="$ctrl.clear()" class="btn umb-button--xs dropdown-toggle umb-button-group__toggle" style="margin-left: -2px; padding-bottom:0px"><umb-icon icon="icon-wrong"></umb-icon></button></div>',
    controller: Datepicker
};

},{}],10:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.DayRangeComponent = void 0;
class DayRange {
    constructor($timeout) {
        this.$timeout = $timeout;
        this.isOpen = false;
    }
    onClose() {
        this.isOpen = false;
        this.onChange({
            range: this.value,
        });
    }
}
const template = `
    <div class="umb-filter">
        <button type="button" class="btn btn-link flex p0" ng-click="$ctrl.isOpen = true">
            <span><localize key="workflow_dateRange">Range (days)</localize>:</span>
            <span class="bold dib umb-filter__label" ng-bind="$ctrl.value"></span>
            <span class="caret" aria-hidden="true"></span>
        </button>

        <umb-dropdown class="pull-right" ng-if="$ctrl.isOpen" on-close="$ctrl.onClose()" style="z-index:9999">
            <umb-dropdown-item >
                <label for="date-range" class="sr-only">Date range (days)</label>
                <input type="number"
                        class="mb-0"
                        id="date-range"
                        ng-model="$ctrl.value"
                        ng-model-options="{ debounce: 300 }"
                        no-dirty-check />
            </umb-dropdown-item>
        </umb-dropdown>        
    </div>`;
exports.DayRangeComponent = {
    name: 'workflowDayRange',
    transclude: true,
    template: template,
    bindings: {
        onChange: '&',
        value: '='
    },
    controller: DayRange
};

},{}],11:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WorkflowDiffComponent = void 0;
class WorkflowDiff {
    constructor(plmbrWorkflowResource) {
        this.$onInit = () => {
            this.workflowResource.getDiff(this.guid)
                .then(resp => {
                this.currentVersions = resp.diffs.currentVariants;
                this.workflowVersions = resp.diffs.workflowVariants;
                if (this.currentVersions.length === 1) {
                    this.currentVersion = this.currentVersions[0];
                }
                if (this.workflowVersions.length === 1) {
                    this.workflowVersion = this.workflowVersions[0];
                }
                if (this.workflowVersion && this.currentVersion) {
                    this.createDiff();
                }
                this.loading = false;
            });
        };
        this.workflowResource = plmbrWorkflowResource;
        this.loading = true;
    }
    createDiff() {
        this.diffs = [];
        if (this.currentVersion.name !== this.workflowVersion.name) {
            this.diffs.push({
                label: 'Name',
                diff: Diff.diffWords(this.currentVersion.name || '', this.workflowVersion.name || ''),
                isObject: false
            });
        }
        // extract all properties from the tabs and create new object for the diff
        this.workflowVersion.tabs.forEach((tab, tabIndex) => {
            tab.properties.forEach((workflowProperty, propertyIndex) => {
                let currentProperty = this.currentVersion.tabs.length ?
                    this.currentVersion.tabs[tabIndex].properties[propertyIndex] :
                    { value: '' };
                // we have to make properties storing values as object into strings (Grid, nested content, etc.)
                if (workflowProperty.value instanceof Object) {
                    workflowProperty.value = JSON.stringify(workflowProperty.value, null, 1);
                    workflowProperty.isObject = true;
                }
                if (currentProperty.value instanceof Object) {
                    currentProperty.value = JSON.stringify(currentProperty.value, null, 1);
                    currentProperty.isObject = true;
                }
                // diff requires a string
                workflowProperty.value = workflowProperty.value ? workflowProperty.value + '' : '';
                currentProperty.value = currentProperty.value ? currentProperty.value + '' : '';
                const diff = Diff.diffWords(currentProperty.value, workflowProperty.value);
                this.diffs.push({
                    label: workflowProperty.label,
                    diff: diff,
                    isObject: (workflowProperty.isObject || currentProperty.isObject) ? true : false
                });
            });
        });
    }
    variantDiff() {
        this.workflowVersion = this.workflowVersions.find(x => x.language.culture === this.activeVariant.culture);
        this.currentVersion = this.currentVersions.find(x => x.language.culture === this.activeVariant.culture);
        // if no current version, assume it's an unpublished variant, so all values are new
        if (!this.currentVersion) {
            this.currentVersion = {
                name: '',
                tabs: []
            };
        }
        this.createDiff();
    }
    hasDiffs() {
        return this.workflowVersions && this.workflowVersions.some(x => x.name !== null && x.tabs.length)
            && this.currentVersions && this.currentVersions.some(x => x.name !== null && x.tabs.length);
    }
}
exports.WorkflowDiffComponent = {
    name: 'workflowDiff',
    transclude: true,
    template:'<div class="diff workflow"><div ng-if="!$ctrl.loading"><div ng-if="$ctrl.workflowVersions.length > 1" class="mb-2"><small class="d-block mb-2"><localize key="workflow_diffVariants">The active workflow includes multiple content variants. Select the language below to view the changes for each variant.</localize></small><select ng-options="l.language as l.language.name for l in $ctrl.workflowVersions" ng-model="$ctrl.activeVariant" ng-change="$ctrl.variantDiff()"></select></div><div ng-if="$ctrl.hasDiffs()"><table class="table table-condensed table-bordered"><tbody><tr ng-repeat="d in $ctrl.diffs track by $index"><td class="bold" ng-bind="d.label"></td><td ng-class="{\'pre-line\': d.isObject, \'word-wrap\': !d.isObject}"><span ng-repeat="part in d.diff"><ins ng-if="part.added" ng-bind="part.value"></ins><del ng-if="part.removed" ng-bind="part.value"></del><span ng-if="!part.added && !part.removed" ng-bind="part.value"></span></span></td></tr></tbody></table><small class="d-block"><localize key="workflow_diffHelp">The table above shows the differences between the current published version and the pending changes in this workflow.<br><br><del>Red text</del>will be removed.<ins>Green text</ins>will be added.</localize></small></div><div ng-if="!$ctrl.hasDiffs()"><small class="d-block"><localize key="workflow_diffNoVersions">Unable to find versions for comparison. Maybe they\'ve been deleted? Try Umbraco\'s Rollback tool to view all available change history for this item.</localize></small></div></div><umb-load-indicator ng-show="$ctrl.loading"></umb-load-indicator></div>',
    bindings: {
        guid: '<',
    },
    controller: WorkflowDiff
};

},{}],12:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.generateFilters = exports.FilterPickerComponent = void 0;
const constants_1 = require("../../js/constants");
class FilterPicker {
    constructor(editorService, dateHelper, localizationService) {
        this.editorService = editorService;
        this.dateHelper = dateHelper;
        this.filters = {};
        localizationService.localize('workflow_filters')
            .then(resp => this.filterStr = resp);
    }
    showFilters() {
        const overlayModel = {
            view: `${Umbraco.Sys.ServerVariables.Plumber.viewsPath}overlays/filterpicker.overlay.html`,
            size: constants_1.constants.sizes.s,
            title: this.filterStr,
            filters: this.filters,
            disabledFilters: this.disabledFilters,
            nodeView: this.nodeView,
            submit: filters => {
                this.filters = filters;
                generateFilters(this.filters, this.nodeView, this.dateHelper);
                this.onChange({
                    filters: this.filters,
                });
                this.editorService.close();
            },
            close: () => this.editorService.close()
        };
        this.editorService.open(overlayModel);
    }
}
const template = `
    <div class="umb-filter mr-2">
        <button type="button" class="btn btn-link flex p0" ng-click="$ctrl.showFilters()">
            <span>{{ $ctrl.filterStr }}: </span>
            <span class="bold dib umb-filter__label">{{ $ctrl.filters.count || '0' }}</span>
            <span class="caret" aria-hidden="true"></span>
        </button>
    </div>`;
exports.FilterPickerComponent = {
    name: 'workflowFilterPicker',
    transclude: true,
    template: template,
    bindings: {
        onChange: '&',
        filters: '=',
        disabledFilters: '<',
        nodeView: '<',
    },
    controller: FilterPicker
};
/**
     * parse the filter object into comma-separated key and value strings
     * */
function generateFilters(filters, nodeView, dateHelper) {
    var _a;
    let keys = [];
    let values = [];
    let now = dateHelper.convertToServerStringTime(moment(new Date()), Umbraco.Sys.ServerVariables.application.serverTimeOffset);
    if (filters.node) {
        keys.push('nodeId');
        values.push(filters.node.id);
    }
    if (filters.user) {
        keys.push('authorUserId');
        values.push(filters.user.id);
    }
    if (filters.variant) {
        keys.push('variant');
        values.push(filters.variant);
    }
    if (filters.type) {
        keys.push('type');
        values.push(filters.type);
    }
    if ((_a = filters.status) === null || _a === void 0 ? void 0 : _a.length) {
        keys.push('status');
        values.push(filters.status.join('|'));
    }
    if (filters.createdFrom) {
        keys.push('createdDate');
        values.push(filters.createdFrom + '|' + (filters.createdTo || now));
    }
    if (filters.completedFrom) {
        keys.push('completedDate');
        values.push(filters.completedFrom + '|' + (filters.completedTo || now));
    }
    filters.keys = keys.join(',');
    filters.values = values.join(',');
    filters.count = (keys.length - (nodeView ? 1 : 0)) || 0;
}
exports.generateFilters = generateFilters;

},{"../../js/constants":22}],13:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WorkflowGroupPickerComponent = void 0;
class WorkflowGroupPicker {
    constructor(plmbrGroupsResource) {
        this.loading = false;
        this.workflowGroupsResource = plmbrGroupsResource;
    }
    $onInit() {
        if (!this.model.selection) {
            this.model.selection = [];
        }
        this.loading = true;
        this.workflowGroupsResource.getAllSlim()
            .then(groups => {
            this.groups = groups.items;
            this.updateAvailableGroups();
            this.loading = false;
        });
    }
    updateAvailableGroups() {
        this.groups.forEach(g => {
            var _a;
            g.selected = (_a = this.model.approvalPath) === null || _a === void 0 ? void 0 : _a.some(x => x.groupId === g.groupId);
        });
    }
    selectUserGroup(group) {
        if (!group.selected) {
            group.selected = true;
            this.model.selection.push(group);
        }
        else {
            const idx = this.model.selection.findIndex(x => x.groupId === group.groupId);
            if (idx !== -1) {
                this.model.selection[idx].selected = false;
                this.model.selection.splice(idx, 1);
            }
        }
    }
}
exports.WorkflowGroupPickerComponent = {
    name: 'workflowGroupPicker',
    transclude: true,
    template:'<umb-load-indicator ng-if="$ctrl.loading"></umb-load-indicator><umb-box><umb-box-content><div class="mb3"><umb-search-filter model="searchTerm" label-key="placeholders_filter" text="Type to filter..." css-class="w-100" auto-focus="true"></umb-search-filter></div><div class="umb-user-group-picker-list"><div class="umb-user-group-picker-list-item" ng-repeat="group in $ctrl.groups | filter:searchTerm"><button type="button" class="umb-user-group-picker__action" ng-click="$ctrl.selectUserGroup(group)"><span class="sr-only" ng-if="!group.selected"><localize key="buttons_select">Select</localize>{{group.name}}</span> <span class="sr-only" ng-if="group.selected">{{group.name}}<localize key="general_selected">Selected</localize></span></button><div class="umb-user-group-picker-list-item__icon"><umb-icon ng-if="!group.selected" icon="{{ group.icon }}" class="{{ group.icon }}" aria-hidden="true"></umb-icon><umb-checkmark ng-if="group.selected" checked="group.selected" size="xs"></umb-checkmark></div><div><div class="umb-user-group-picker-list-item__name">{{ group.name }}</div><div class="umb-user-group-picker-list-item__permission" ng-if="group.description"><span>{{ group.description }}</span></div></div></div></div><umb-empty-state ng-if="$ctrl.groups.length === 0 && !$ctrl.loading" position="center"><localize key="user_noUserGroupsAdded">No user groups have been added</localize></umb-empty-state></umb-box-content></umb-box>',
    bindings: {
        model: '='
    },
    controller: WorkflowGroupPicker
};

},{}],14:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.HistoryComponent = void 0;
const constants_1 = require("../../js/constants");
const sorter_1 = require("../../js/models/sorter");
class History {
    constructor($scope, $rootScope, editorState, plmbrWorkflowResource) {
        this.nodeView = false;
        this.disabledFilters = [];
        this.perPage = 10;
        /**
         *
         */
        this.fetch = (perPage = this.perPage) => {
            this.model = {
                perPage,
                currentPage: 1,
                filters: this.filters,
                nodeView: this.nodeView,
                handler: this.workflowResource.getAllInstances,
                direction: sorter_1.SortDirection.DESC,
            };
        };
        this.workflowResource = plmbrWorkflowResource;
        this.activityFilter = this.workflowResource.getActivityFilter();
        this.workflowResource.setTreeState();
        if (this.activityFilter !== null && Object.values(this.activityFilter).length > 0) {
            this.filters = this.activityFilter;
        }
        else {
            this.filters = {
                node: editorState.getCurrent(),
                status: []
            };
        }
        this.nodeView = !!this.filters.node;
        this.onActioned = $rootScope.$on(constants_1.constants.events.workflowActioned, (_, data) => {
            if (this.filters.node && this.filters.node.id === data.nodeId) {
                this.fetch();
            }
        });
        this.fetch();
        $scope.$on('$destroy', () => {
            this.onActioned();
            this.workflowResource.setActivityFilter(null);
        });
    }
    onFilter(filters) {
        this.filters = filters;
        this.fetch();
    }
}
exports.HistoryComponent = {
    name: 'workflowHistory',
    template:'<div class="workflow workflow-history"><umb-box><umb-box-header><div class="flex"><workflow-filter-picker filters="$ctrl.filters" disabled-filters="$ctrl.disabledFilters" on-change="$ctrl.onFilter(filters)" nodeView="$ctrl.nodeView"></workflow-filter-picker><workflow-page-size on-change="$ctrl.fetch(perPage)" value="$ctrl.perPage"></workflow-page-size></div></umb-box-header><umb-box-content><workflow-instances model="$ctrl.model"></workflow-instances></umb-box-content></umb-box></div>',
    bindings: {
        disabledFilters: '=',
    },
    controller: History
};

},{"../../js/constants":22,"../../js/models/sorter":31}],15:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WorkflowInstancesComponent = void 0;
const models_1 = require("../../js/models");
const tasklist_base_1 = require("../../js/models/tasklist.base");
const filterpicker_1 = require("../filterpicker/filterpicker");
class WorkflowInstances extends tasklist_base_1.TaskListBase {
    constructor($scope, $rootScope, plmbrActionsService, notificationsService, dateHelper) {
        super($scope, $rootScope, notificationsService, plmbrActionsService);
        this.dateHelper = dateHelper;
        this.sorter = new models_1.Sorter(() => this.fetch());
        this.pagination = new models_1.Pagination(() => this.fetch(), 5);
        this.sort = (key) => this.sorter.update(key);
    }
    $onChanges(change) {
        if (change.model.currentValue) {
            this.model = change.model.currentValue;
            this.pagination.perPage = this.model.perPage || this.pagination.perPage;
            // allow setting up/down since history should be oldest first, others should be newest
            if (this.model.direction) {
                this.sorter.setDirection(this.model.direction);
            }
            this.fetch();
        }
    }
    fetch() {
        var _a, _b;
        const query = {
            page: this.pagination.pageNumber,
            count: this.pagination.perPage,
            sortBy: this.sorter.sortBy,
            sortDirection: this.sorter.sortDirectionString,
        };
        // if filters are passed down from the history view, ensure they are up to date
        filterpicker_1.generateFilters(this.model.filters, this.model.nodeView, this.dateHelper);
        if ((_a = this.model.filters) === null || _a === void 0 ? void 0 : _a.keys)
            query.filters = this.model.filters.keys;
        if ((_b = this.model.filters) === null || _b === void 0 ? void 0 : _b.values)
            query.filterValues = this.model.filters.values;
        if (this.model.groupId)
            query.groupId = this.model.groupId;
        this.doFetch(query, this.pagination);
    }
    ;
    statusColor(item) {
        switch (item.cssStatus) {
            case 'approved':
                return 'success'; //approved
            case 'not':
            case 'cancelled':
                return 'gray'; //cancelled
            case 'errored':
                return 'danger'; //error 
            case 'rejected':
                return 'warning';
            default:
                return 'primary'; //resubmitted, pending
        }
    }
}
exports.WorkflowInstancesComponent = {
    name: 'workflowInstances',
    transclude: true,
    template:'<div class="relative workflow-tasks"><div ng-show="$ctrl.items.length && !$ctrl.paging"><div class="umb-table"><div class="umb-table-head"><div class="umb-table-row"><div class="umb-table-cell umb-table__name not-fixed"><localize key="headers_page">Page</localize><span ng-if="$ctrl.languageCount > 1">&nbsp;(<localize key="general_language">Language</localize>)</span></div><div class="umb-table-cell"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'type\')"><localize key="content_type">Type</localize><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'type\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'type\'] }}"></umb-icon></button></div><div class="umb-table-cell"><localize key="workflow_requestedBy">Requested by</localize></div><div class="umb-table-cell"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'id\')"><localize key="workflow_requestedOn">Requested on</localize><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'id\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'id\'] }}"></umb-icon></button></div><div class="umb-table-cell umb-table-cell__15"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'authorComment\')"><localize key="general_comment">Comment</localize><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'authorComment\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'authorComment\'] }}"></umb-icon></button></div><div class="umb-table-cell"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'status\')"><localize key="general_status">Status</localize><umb-icon class="umb-table-head__icon" aria-hidden="true" ng-if="$ctrl.sorter.sortBy === \'status\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'status\'] }}"></umb-icon></button></div><div class="umb-table-cell umb-table-cell__detail"></div></div></div><div class="umb-table-body"><div class="table-row-outer {{ ::item.cssStatus }}" ng-repeat="item in $ctrl.items track by $index"><div class="umb-table-row"><div class="umb-table-cell umb-table__name not-fixed"><span ng-if="!item.node.exists" ng-bind="::item.node.name"></span> <a href="#" ng-if="item.node.exists" ng-href="#/content/content/edit/{{item.node.id}}?cculture={{$ctrl.variantCode(item)}}">{{ $ctrl.displayName(item) }}</a></div><div class="umb-table-cell" ng-bind="item.type"></div><div class="umb-table-cell" ng-bind="item.requestedBy"></div><div class="umb-table-cell" ng-bind="item.requestedOn"></div><div class="umb-table-cell umb-table-cell__15">{{ (item.instance.comment || item.comment) | limitTo: 140 }} {{ (item.instance.comment || item.comment).length > 140 ? \'...\' : \'\' }}</div><div class="umb-table-cell"><umb-badge color="{{ $ctrl.statusColor(item) }}" size="xs" title="{{ item.statusName || item.status }}" ng-bind="item.statusName || item.status"></umb-badge></div><div class="umb-table-cell umb-table-cell__detail show-overflow"><umb-button action="$ctrl.detail(item)" type="button" button-style="info" add-ellipsis="true" label-key="workflow_detail"></umb-button></div></div></div></div></div></div><div class="flex justify-center" ng-show="$ctrl.items && $ctrl.loaded"><umb-pagination ng-if="$ctrl.pagination.totalPages > 1" page-number="$ctrl.pagination.pageNumber" total-pages="$ctrl.pagination.totalPages" on-next="$ctrl.pagination.goToPage" on-prev="$ctrl.pagination.goToPage" on-go-to-page="$ctrl.pagination.goToPage"></umb-pagination></div><umb-empty-state ng-if="!$ctrl.items.length && $ctrl.loaded"><localize key="content_listViewNoItems">There are no items show in the list.</localize></umb-empty-state><umb-load-indicator ng-if="!$ctrl.loaded || $ctrl.paging"></umb-load-indicator></div>',
    bindings: {
        model: '<',
    },
    controller: WorkflowInstances
};

},{"../../js/models":29,"../../js/models/tasklist.base":33,"../filterpicker/filterpicker":12}],16:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PageSizeComponent = void 0;
class PageSize {
    constructor() {
        this.value = 10;
        this.isOpen = false;
        this.options = [5, 10, 20, 25];
    }
    select(perPage) {
        this.isOpen = false;
        this.value = perPage;
        this.onChange({ perPage });
    }
}
const template = `
    <div class="umb-filter">
        <button type="button" class="btn btn-link flex p0" ng-click="$ctrl.isOpen = true">
            <span><localize key="workflow_pageSize">Page size</localize>:</span>
            <span class="bold dib umb-filter__label" ng-bind="$ctrl.value"></span>
            <span class="caret" aria-hidden="true"></span>
        </button>

        <umb-dropdown class="pull-right" ng-if="$ctrl.isOpen" on-close="$ctrl.isOpen = false" style="z-index:9999">
            <umb-dropdown-item ng-repeat="option in $ctrl.options">
                <button type="button" ng-click="$ctrl.select(option)" ng-bind="::option"></button>                
            </umb-dropdown-item>
        </umb-dropdown>
    </div>`;
exports.PageSizeComponent = {
    name: 'workflowPageSize',
    transclude: true,
    template: template,
    bindings: {
        onChange: '&',
        value: '='
    },
    controller: PageSize
};

},{}],17:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ProgressComponent = void 0;
class WorkflowProgress {
    constructor(localizationService) {
        this.localizationService = localizationService;
        this.permissions = [];
        this.totalSteps = 0;
        this.addPendingTasks = () => {
            // can't count steps, as reject/resubmit use the same step number - instead, get unique step numbers
            const activeSteps = this.tasks.map(t => t.currentStep).filter((v, i, s) => s.indexOf(v) === i);
            // if permissions are missing (group deleted), stuff a mock permission in at the correct index
            if (this.permissions.length < this.totalSteps) {
                let missingIndexes = [];
                this.permissions.forEach(p => {
                    missingIndexes[p.permission] = p;
                });
                for (let i = 0; i < missingIndexes.length; i += 1) {
                    if (!missingIndexes[i]) {
                        this.permissions.splice(i, 0, {
                            groupName: 'Group does not exist'
                        });
                    }
                }
            }
            if (activeSteps.length < this.totalSteps) {
                for (let i = activeSteps.length; i < this.totalSteps; i += 1) {
                    this.tasks.push({
                        statusName: this.pendingApprovalStr,
                        currentStep: i,
                        groupName: this.permissions[i] ? this.permissions[i].groupName : ''
                    });
                }
            }
            // if a rejected task is the last in its step, stuff a fake pending resubmission and pending approval task
            // the responsible user will be the requestedBy value on the task
            this.tasks.forEach((task, i) => {
                if (task.status === 2 && (this.tasks[i + 1] && this.tasks[i + 1].currentStep === task.currentStep + 1 || !this.tasks[i + 1])) {
                    this.tasks.splice(i + 1, 0, {
                        statusName: this.awaitingResubmissionStr,
                        currentStep: task.currentStep,
                        cssStatus: 'pendingapproval',
                        groupName: task.requestedBy
                    });
                }
            });
            // once everything is up to date, iterate again, and set the group classes
            // if a rejection string hits more than 5 stages, collapse these to avoid the progress bar growing horribly
            this.tasks.forEach((task, i) => {
                const prev = this.tasks[i - 1];
                const next = this.tasks[i + 1];
                let str = '';
                if (prev && task.currentStep !== prev.currentStep || !prev) {
                    str = 'grouped-start';
                }
                if (next) {
                    str += task.currentStep !== next.currentStep ? ' grouped-end' : ' grouped';
                }
                else {
                    str += ' grouped-end';
                }
                task.groupClass = str;
            });
            // and iterate again :( to find each set of grouped tasks, remove the extras
            // to be left with grouped-start, collapsed, grouped-end
            this.tasks.forEach((task, i) => {
                if (task.groupClass !== 'grouped-start grouped')
                    return;
                // find the next grouped-end task
                const endTaskIndex = this.tasks.findIndex((t, j) => { var _a; return j > i && ((_a = t.groupClass) === null || _a === void 0 ? void 0 : _a.includes('grouped-end')); });
                // if it's more than two tasks away, we collapse to three tasks
                if (endTaskIndex - i < 3)
                    return;
                const pad = {
                    cssStatus: 'collapsed',
                    groupClass: 'grouped',
                    currentStep: 0,
                    groupName: (this.plusMoreStr || 'plus %0% more').replace('%0%', (endTaskIndex - i - 1).toString())
                };
                this.tasks.splice(i + 1, 0, pad);
                this.tasks.splice(i + 2, endTaskIndex - i - 1);
            });
        };
        this.$onChanges = changes => {
            if (changes.instance.currentValue) {
                const instance = changes.instance.currentValue;
                this.tasks = instance.tasks.sort((a, b) => a.currentStep > b.currentStep);
                this.totalSteps = instance.totalSteps;
                this.permissions = instance.permissions;
                this.localizationService.localizeMany(['workflow_pendingApproval', 'workflow_awaitingResubmission', 'workflow_plusMore'])
                    .then((resp) => {
                    this.pendingApprovalStr = resp[0];
                    this.awaitingResubmissionStr = resp[1];
                    this.plusMoreStr = resp[2];
                    this.addPendingTasks();
                });
            }
        };
    }
}
const template = `          
    <div ng-repeat="task in $ctrl.tasks"         
        class="progress-step {{ task.cssStatus || 'future-step' }}">
        <div class="{{ task.groupClass }}">
            <span class="marker">
                <i class="icon-"></i>
            </span>
            <span class="tooltip {{ ::task.cssStatus }}">
                <span ng-bind="::task.statusName"></span>
                {{ ::(task.status === 7 ? task.completedBy : task.groupName )}}
            </span>
        </div>
    </div>`;
exports.ProgressComponent = {
    name: 'workflowProgress',
    transclude: true,
    bindings: {
        instance: '<',
    },
    template: template,
    controller: WorkflowProgress
};

},{}],18:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SchedulingComponent = void 0;
exports.SchedulingComponent = {
    name: 'workflowScheduling',
    transclude: true,
    template: ` 
        <umb-box class="workflow"> 
            <umb-box-header title-key="workflow_scheduling"></umb-box-header>
            <umb-box-content>
                <p ng-bind="::$ctrl.item.type"></p>
                <div class="d-flex items-center" ng-if="::$ctrl.scheduledDatePassed">
                    <umb-icon icon="icon-alert" class="schedule-warning-icon"></umb-icon>
                    <small>
                        <localize key="workflow_schedulePassed">Scheduled date passed before the workflow was completed. Content will be released when the current workflow is completed.</localize> 
                    </small>
                </div>
            </umb-box-content>
        </umb-box>`,
    bindings: {
        item: '<'
    },
    controller: class WorkflowScheduling {
        $onInit() {
            const scheduledDate = new Date(this.item.scheduledDate);
            const now = new Date();
            this.scheduledDatePassed = scheduledDate < now && this.item.status === 3;
        }
    }
};

},{}],19:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SubmitWorkflowComponent = void 0;
const constants_1 = require("../../js/constants");
class WorkflowSubmit {
    constructor($scope, $timeout, $rootScope, $element, $sce, $location, editorState, editorService, dateHelper, fileManager, userService, formHelper, notificationsService, contentResource, localizationService, languageResource, navigationService, plmbrActionsService) {
        this.$timeout = $timeout;
        this.$rootScope = $rootScope;
        this.$element = $element;
        this.$sce = $sce;
        this.$location = $location;
        this.editorState = editorState;
        this.editorService = editorService;
        this.dateHelper = dateHelper;
        this.fileManager = fileManager;
        this.formHelper = formHelper;
        this.notificationsService = notificationsService;
        this.contentResource = contentResource;
        this.localizationService = localizationService;
        this.languageResource = languageResource;
        this.navigationService = navigationService;
        this.events = () => {
            // this.state is null when submit initially loads after a process completes
            this.onButtonStateChanged = this.$rootScope.$on(constants_1.constants.events.buttonStateChanged, (_, data) => {
                if (this.state && data.id === this.state.nodeId) {
                    this.buttonState = data.state;
                    this.setSubButtons();
                    this.setDefaultButton();
                }
            });
            this.onAppActive = this.$rootScope.$on(constants_1.constants.events.workflowAppActive, (_, data) => {
                if (this.state && data.id === this.state.nodeId) {
                    this.action = data.action || constants_1.constants.actions.publish;
                    // if we need workflowed unpublish actions, set the sub button to the opposite of the current action
                    this.setSubButtons();
                    this.setDefaultButton();
                }
            });
            this.onActioned = this.$rootScope.$on(constants_1.constants.events.workflowActioned, () => {
                this.comment = null;
            });
        };
        this.setSubButtons = () => {
            // if we need workflowed unpublish actions, set the sub button to the opposite of the current action
            // if no unpublish permissions exist, disable the workflow and show the overlay
            if (this.state.requireUnpublish) {
                let subButton = this.action === constants_1.constants.actions.publish && this.state.hasUnpublishPermissions ? this.unpublishBtn
                    : this.action === constants_1.constants.actions.unpublish ? this.publishBtn
                        : undefined;
                this.subButtons = subButton ? [subButton] : [];
                this.noUnpublishPermissions = this.action === constants_1.constants.actions.unpublish && !this.state.hasUnpublishPermissions;
            }
            else {
                this.subButtons = [];
                this.noUnpublishPermissions = false;
            }
        };
        this.setDefaultButton = () => {
            this.defaultButton = this.action === constants_1.constants.actions.publish ? this.publishBtn : this.unpublishBtn;
        };
        this.$onInit = () => {
            this.labelKey = this.action === constants_1.constants.actions.publish ? 'workflow_describeChanges' : 'workflow_addComment';
            this.templateKey = `workflowCommentTemplates_${this.editorState.current.contentTypeAlias}`;
            this.localizationService.localizeMany([
                'workflow_invalidContent',
                'workflow_noUnpublishPermissions'
            ])
                .then(result => {
                let [invalidContent, noUnpublishPermissions] = result;
                this.invalidContentStr = invalidContent;
                if (noUnpublishPermissions) {
                    document.body.style.setProperty('--plumberNoUnpublishPermissions', `'${noUnpublishPermissions}'`);
                }
                this.publishBtn = {
                    labelKey: 'workflow_publishButton',
                    buttonStyle: constants_1.constants.states.success,
                    handler: () => this.initiate(true),
                    shortcut: 'ctrl+p'
                };
                this.unpublishBtn = {
                    labelKey: 'workflow_unpublishButton',
                    buttonStyle: constants_1.constants.states.success,
                    handler: () => this.initiate(false),
                    shortcut: 'ctrl+u'
                };
                this.setSubButtons();
                this.setDefaultButton();
            });
        };
        this.setScheduledDate = (variant, isPublish) => {
            if (!this.scheduledDate)
                return;
            const dateToSet = isPublish ? 'releaseDate' : 'expireDate';
            if (variant[dateToSet] === this.scheduledDate) {
                return;
            }
            variant[dateToSet] = this.scheduledDate;
            variant[`${dateToSet}Formatted`] = this.scheduledDateFormatted;
            variant.isDirty = true;
        };
        this.initiate = (publish) => {
            let variant = [];
            if (!this.formHelper.submitForm({
                scope: this.scope,
                action: 'save'
            })) {
                this.notificationsService.warning(this.invalidContentStr);
                return;
            }
            // if content is dirty, save it then call back into this function
            // since content was saved, it's no longer dirty, so we won't hit this.
            if (this.scope.contentForm.$dirty) {
                // if nested content exists, it needs to be syncd back
                // can trigger this by closing any open nc instances
                // it's horrible, but it works. Lots of timeouts, these are required
                // to allow changes to propagate before acting on the expected data
                const ncHeaders = document.querySelectorAll('.umb-nested-content__item--active .umb-nested-content__header-bar');
                for (let openNc of Object.values(ncHeaders)) {
                    this.$timeout(() => openNc.click());
                }
                this.buttonState = constants_1.constants.states.busy;
                this.$timeout(() => {
                    this.scope.content.variants.forEach(v => v.save = v.isDirty || v.active);
                    let isNew = this.scope.content.id === 0;
                    // need to store the date to check the auto-save completed
                    // there's no other way to identify a successful, other than checking
                    // notifications, but that relies on language, and knowing the notification text
                    const updateDate = this.scope.content.updateDate;
                    this.contentResource.save(this.scope.content, isNew, this.fileManager.getFiles(), false)
                        .then(newNode => {
                        // update date didn't change, save was cancelled or failed
                        if (newNode.updateDate === updateDate) {
                            this.buttonState = constants_1.constants.states.init;
                            // notifications aren't displayed unless we ask for them...
                            newNode.notifications.forEach(n => this.notificationsService.add(n));
                            return;
                        }
                        this.$element.inheritedData('$formController').$setPristine();
                        if (isNew) {
                            this.scope.content.id = newNode.id;
                            this.state.nodeId = newNode.id;
                            //clear the query strings
                            this.navigationService.clearSearch(["cculture", "csegment"]);
                            this.navigationService.setSoftRedirect();
                            //change to new path
                            this.$location.path(`/content/content/edit/${this.scope.content.id}`);
                            //don't add a browser history for this
                            this.$location.replace();
                            this.navigationService.syncTree({
                                tree: 'content',
                                path: newNode.path.split(',').map(x => parseInt(x)),
                                activate: true,
                            }).then(() => this.initiate(publish));
                        }
                        else {
                            this.initiate(publish);
                        }
                    });
                });
            }
            else {
                // provide variants as an array
                if (this.selectedVariantName === 'all') {
                    variant = this.variants.map(v => v.language.culture);
                }
                else {
                    variant = [this.selectedVariantName];
                }
                // if release date has changed, assign it to all variants 
                // this is a UI change only as the schedule is updated on the server
                // as part of creating the workflow instance
                if (variant.includes('*')) {
                    this.scope.content.variants.forEach(v => this.setScheduledDate(v, publish));
                }
                else {
                    variant.forEach(variantName => this.setScheduledDate(this.scope.content.variants.find(v => v.language.culture === variantName), publish));
                }
                this.actionsService.initiate({
                    nodeId: this.scope.content.id,
                    contentTypeId: this.scope.content.contentTypeId,
                    comment: this.comment,
                    scheduledDate: this.scheduledDate,
                    publish,
                    variant,
                    attachmentId: this.attachment ? this.attachment.id : null
                });
                this.comment = null;
            }
        };
        this.filepicker = () => {
            let currentSelection;
            angular.copy(this.attachment, currentSelection);
            const filePickerOptions = {
                selection: currentSelection,
                multiPicker: false,
                submit: model => {
                    this.attachment = model.selection[0];
                    this.editorService.close();
                },
                close: () => {
                    this.editorService.close();
                }
            };
            this.editorService.mediaPicker(filePickerOptions);
        };
        this.$onChanges = changes => {
            var _a, _b;
            if (changes.state && changes.state.currentValue) {
                if (!this.state.isActive) {
                    this.comment = null;
                }
            }
            // when state is updated, not intialized, reset the buttons
            if (changes.state && changes.state.previousValue) {
                this.setSubButtons();
                this.setDefaultButton();
            }
            if (changes.scope && changes.scope.currentValue) {
                this.variants = this.scope.content.variants;
                // invariant node
                if (this.variants.length === 1 && this.variants[0].language === null) {
                    this.selectedVariantName = '*';
                }
                // variant node 
                else {
                    const isCreate = this.$location.search().create === 'true';
                    if (this.variants && this.variants.filter(x => x.language).length && !isCreate) {
                        this.currentVariant = this.variants.find(v => v.active);
                        this.defaultVariant = this.variants.find(v => v.language.isDefault);
                        this.selectedVariantName = (_b = (_a = this.currentVariant) === null || _a === void 0 ? void 0 : _a.language.culture) !== null && _b !== void 0 ? _b : ''; // set the selection to the current displayed variant
                        this.localizeToggleLabels();
                    }
                    else {
                        this.languageResource.getAll()
                            .then(resp => {
                            const defaultLang = resp.find(lang => lang.isDefault);
                            // this should never happen, but keeps TS happy
                            if (!defaultLang)
                                return;
                            this.currentVariant = {
                                language: defaultLang
                            };
                            this.defaultVariant = {
                                language: defaultLang
                            };
                            this.selectedVariantName = this.currentVariant.language.culture;
                            this.localizeToggleLabels();
                        });
                    }
                }
            }
        };
        this.localizeToggleLabels = () => {
            this.localizationService.localizeMany([
                'workflow_thisVariant',
                'workflow_invariant',
                'workflow_multiVariant',
                'workflow_thisVariantDesc',
                'workflow_invariantDesc',
                'workflow_multiVariantDesc'
            ])
                .then((data) => {
                var _a, _b, _c, _d;
                let [thisVariant = 'Just this one', invariant = 'Invariant', multiVariant = 'Multi-variant', thisVariantDesc, invariantDesc, multiVariantDesc] = data;
                let obj = { thisVariant, invariant, multiVariant, thisVariantDesc, invariantDesc, multiVariantDesc };
                // append the current/default variant details to the description
                obj.thisVariantDesc = obj.thisVariantDesc.replace('%0%', (_b = (_a = this.currentVariant) === null || _a === void 0 ? void 0 : _a.language.name) !== null && _b !== void 0 ? _b : '');
                obj.invariantDesc = obj.invariantDesc.replace('%0%', (_d = (_c = this.defaultVariant) === null || _c === void 0 ? void 0 : _c.language.name) !== null && _d !== void 0 ? _d : '');
                this.workflowTypeLabels = obj;
            });
        };
        this.actionsService = plmbrActionsService;
        // action is set when selecting the footer button
        // but the workflow type may differ if the user selects from the sub-buttons
        // the button handler sets isPublish and is used in initiate()
        this.action = constants_1.constants.actions.publish;
        this.events();
        var now = new Date();
        var nowFormatted = moment(now).format("YYYY-MM-DD HH:mm");
        this.datepickerConfig = {
            enableTime: true,
            dateFormat: "Y-m-d H:i",
            time_24hr: true,
            minDate: nowFormatted,
            defaultDate: null,
            defaultHour: now.getHours(),
            defaultMinute: now.getMinutes() + 5
        };
        // ensure defaults for scheduling match the defaults for content
        // ie scheduleDate and scheduledDateFormatted should be null
        this.datepickerClear();
        userService.getCurrentUser().then(currentUser => this.currentUser = currentUser);
        $scope.$on('$destroy', () => {
            this.onButtonStateChanged();
            this.onAppActive();
            this.onActioned();
        });
    }
    datepickerChange(dateStr) {
        this.scheduledDate = this.dateHelper.convertToServerStringTime(moment(dateStr), Umbraco.Sys.ServerVariables.application.serverTimeOffset);
        this.scheduledDateFormatted = this.dateHelper.getLocalDate(this.scheduledDate, this.currentUser.locale, "MMM Do YYYY, HH:mm");
    }
    datepickerClear() {
        this.scheduledDate = null;
        this.scheduledDateFormatted = null;
    }
    filepickerClear() {
        delete this.attachment;
    }
}
exports.SubmitWorkflowComponent = {
    name: 'workflowSubmit',
    transclude: true,
    bindings: {
        state: '<',
        scope: '<',
    },
    template:'<div class="workflow"><div class="flex justify-between flex-wrap"><umb-box ng-class="{\'active\' : $ctrl.comment.length, \'no-unpublish-permissions\' : $ctrl.noUnpublishPermissions }" class="umb-box--half d-flex flex-column pos-relative"><umb-box-header title-key="workflow_initiateWorkflow"></umb-box-header><umb-box-content class="d-flex flex-column flex-1"><workflow-comments comment="$ctrl.comment" template-key="$ctrl.templateKey" label-key="$ctrl.labelKey" max-length="250" invalid="$ctrl.invalidComment"></workflow-comments><div class="flex mb-3 date-wrapper__date" ng-if="$ctrl.state.allowAttachments"><label><localize key="workflow_attachment">Attachment (optional)</localize></label><div class="flex" style="font-size:15px"><div><button type="button" ng-click="$ctrl.filepicker()" ng-hide="$ctrl.attachment" class="btn btn-outline-wf"><localize key="workflow_addFile">Add file</localize></button> <button type="button" ng-click="$ctrl.filepicker()" ng-show="$ctrl.attachment" class="btn umb-button--xs" style="outline:none;">{{ $ctrl.attachment.name }}</button></div><button type="button" ng-show="$ctrl.attachment" ng-click="$ctrl.filepickerClear()" class="btn umb-button--xs dropdown-toggle umb-button-group__toggle" style="margin-left: -2px; padding-bottom:0px"><umb-icon icon="icon-wrong"></umb-icon></button></div></div><div class="flex date-wrapper__date mb-3"><label><localize key="workflow_scheduledDate">Scheduled date (optional)</localize></label><div class="btn-group flex" style="font-size:15px"><umb-date-time-picker ng-model="$ctrl.scheduledDate" options="$ctrl.datepickerConfig" on-change="$ctrl.datepickerChange(dateStr)"><div><button type="button" ng-show="$ctrl.scheduledDate" class="btn umb-button--xs" style="outline:none;">{{ $ctrl.scheduledDateFormatted }}</button> <button type="button" class="btn btn-outline-wf" ng-hide="$ctrl.scheduledDate"><localize key="content_setDate">Set date</localize></button></div></umb-date-time-picker><button type="button" ng-show="$ctrl.scheduledDate" ng-click="$ctrl.datepickerClear()" class="btn umb-button--xs dropdown-toggle umb-button-group__toggle" style="margin-left: -2px; padding-bottom:0px"><umb-icon icon="icon-wrong"></umb-icon></button></div></div><small class="d-block mt-1" ng-if="$ctrl.scheduledDate"><localize key="workflow_scheduleDescription">If the scheduled date passes before the workflow is completed, the changes will be published when the final workflow stage has been approved.</localize></small><div class="alert alert-warning mt-auto mb-0" ng-if="$ctrl.scope.contentForm.$dirty"><localize key="workflow_unsavedChanges">Pending content changes will be saved automatically</localize></div></umb-box-content></umb-box><umb-box ng-if="$ctrl.variants.length > 1" ng-class="{\'active\' : $ctrl.selectedVariant != undefined}" class="umb-box--half"><umb-box-header title-key="workflow_selectVariants"></umb-box-header><umb-box-content><ul class="unstyled mb0"><li ng-if="$ctrl.workflowTypeLabels.thisVariant"><umb-toggle checked="$ctrl.selectedVariantName === $ctrl.currentVariant.language.culture" on-click="$ctrl.selectedVariantName = $ctrl.currentVariant.language.culture" show-labels="true" label-position="right" id="workflowType_0" label-on="{{$ctrl.workflowTypeLabels.thisVariant}}" label-off="{{$ctrl.workflowTypeLabels.thisVariant}}"></umb-toggle><div class="mt-1"><small ng-bind="$ctrl.workflowTypeLabels.thisVariantDesc"></small></div></li><li ng-if="$ctrl.state.variantTasks.length"><div class="alert"><small><localize key="workflow_variantsActiveAndUnavailable">Variant workflows unavailable as the following variants have active workflows:</localize>{{ $ctrl.state.variantTasks.join(\',\') }}</small></div></li><li ng-if="$ctrl.workflowTypeLabels.invariant" class="mt-2"><umb-toggle checked="$ctrl.selectedVariantName === \'*\'" on-click="$ctrl.selectedVariantName = \'*\'" show-labels="true" disabled="$ctrl.state.variantTasks.length" label-position="right" id="workflowType_1" label-on="{{$ctrl.workflowTypeLabels.invariant}}" label-off="{{$ctrl.workflowTypeLabels.invariant}}"></umb-toggle><div class="mt-1"><small ng-bind="$ctrl.workflowTypeLabels.invariantDesc"></small></div></li><li ng-if="$ctrl.workflowTypeLabels.multiVariant" class="mt-2"><umb-toggle checked="$ctrl.selectedVariantName === \'all\'" on-click="$ctrl.selectedVariantName = \'all\'" show-labels="true" label-position="right" id="workflowType_2" disabled="$ctrl.state.variantTasks.length" label-on="{{$ctrl.workflowTypeLabels.multiVariant}}" label-off="{{$ctrl.workflowTypeLabels.multiVariant}}"></umb-toggle><div class="mt-1"><small ng-bind="$ctrl.workflowTypeLabels.multiVariantDesc"></small></div></li></ul></umb-box-content></umb-box></div><div class="flex justify-center" ng-class="{\'no-sub-buttons\' : $ctrl.subButtons.length === 0 }" ng-disabled="$ctrl.invalidComment || !$ctrl.selectedVariantName"><umb-button-group button-style="success" default-button="$ctrl.defaultButton" sub-buttons="$ctrl.subButtons" state="$ctrl.buttonState" size="l" direction="up"></umb-button-group></div></div>',
    controller: WorkflowSubmit
};

},{"../../js/constants":22}],20:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TaskListComponent = void 0;
const constants_1 = require("../../js/constants");
class WorkflowTaskList {
    constructor($scope, $rootScope, plmbrWorkflowResource, localizationService) {
        this.tasks = [];
        this.$onChanges = () => {
            this.fetchTasks();
        };
        this.avatarName = (task) => {
            // don't show group if admin completed the task
            if (task.actionedByAdmin) {
                return this.adminStr;
            }
            // if not required, show the group name
            if (task.status === 4) {
                return task.groupName;
            }
            // resubmitted tasks won't have a group, just a user
            if (task.status === 7) {
                return task.completedBy;
            }
            // finally show either the group or the user
            return task.groupName || task.completedBy;
        };
        this.statusColor = (status) => {
            switch (status) {
                case 1:
                    return 'success'; //approved 
                case 2:
                case 6:
                    return 'danger'; //rejected, error
                case 3:
                    return 'primary'; //pending
                case 7:
                    return 'warning'; //resubmitted
                default:
                    return 'gray'; //cancelled, not required
            }
        };
        this.whodunnit = (task) => {
            // if rejected or incomplete, use the group name
            if (task.status === 4 || !task.completedBy) {
                return task.groupName;
            }
            if (task.status === 7) {
                return task.completedBy;
            }
            // if actioned by an admin, show
            if (task.actionedByAdmin) {
                return `${task.completedBy} ${this.asAdminStr} ${this.forStr} ${task.groupName}`;
            }
            // if approved, show the user and group name
            if (task.groupName && task.groupName !== this.noGroupStr) {
                return `${task.completedBy} ${this.forStr} ${task.groupName}`;
            }
            // otherwise, just show the user name
            return task.completedBy;
        };
        /**
         *  there may be multiple tasks for a given step, due to rejection/resubmission - modify the tasks object to nest those tasks
         * @param {array} tasks - the set of existing tasks for the instance. May be less than the total steps required in the workflow
         */
        this.setTaskMeta = (tasks) => {
            if (!tasks.length)
                return;
            tasks.forEach(task => {
                let t = task;
                // push some extra UI strings onto each task
                t.avatarName = this.avatarName(t);
                t.statusColor = this.statusColor(t.status);
                t.whodunnit = this.whodunnit(t);
                if (!this.tasks[t.currentStep]) {
                    this.tasks[t.currentStep] = [];
                }
                this.tasks[t.currentStep].push(t);
            });
            // get last task in last task collection
            let lastTask = this.tasks[this.tasks.length - 1];
            // last will always be a WorkflowTask, as we haven't added any ghosts yet
            let lastStep = lastTask[lastTask.length - 1];
            if (lastStep.status === 2) {
                lastTask.push({
                    avatarName: lastStep.requestedBy,
                    statusColor: this.statusColor(3),
                    statusName: this.awaitingResubmissionStr,
                    whodunnit: lastStep.requestedBy
                });
            }
        };
        /**
         * Add dummy tasks for future workflow stages
         */
        this.addPendingTasks = () => {
            if (!this.permissions || !this.permissions.length)
                return;
            // if permissions are missing (group deleted), stuff a mock permission in at the correct index
            if (this.permissions.length < this.totalSteps) {
                let missingIndexes = [];
                this.permissions.forEach(p => {
                    missingIndexes[p.permission] = p;
                });
                const nodeId = this.permissions[0].nodeId;
                const contentTypeId = this.permissions[0].contentTypeId;
                for (let i = 0; i < missingIndexes.length; i += 1) {
                    if (!missingIndexes[i]) {
                        this.permissions.splice(i, 0, {
                            groupName: this.noGroupStr,
                            permission: i,
                            groupId: -1,
                            variant: '',
                            nodeId,
                            contentTypeId,
                        });
                    }
                }
            }
            if (this.tasks.length < this.totalSteps) {
                for (let i = this.tasks.length; i < this.totalSteps; i += 1) {
                    if (!this.permissions[i])
                        continue;
                    this.tasks.push([{
                            futureTask: true,
                            avatarName: this.permissions[i].groupName,
                            statusName: this.pendingApprovalStr,
                            statusColor: this.statusColor(3),
                            whodunnit: this.permissions[i].groupName,
                            currentStep: i,
                        }]);
                }
            }
        };
        /**
         */
        this.fetchTasks = () => {
            this.workflowResource.getAllTasksByGuid(this.instanceGuid)
                .then(resp => {
                if (resp.notifications) {
                    return;
                }
                this.tasksLoaded = true;
                this.tasks = [];
                this.currentStep = resp.currentStep;
                this.totalSteps = resp.totalSteps;
                this.setTaskMeta(resp.items);
                this.addPendingTasks();
            });
        };
        this.tasksLoaded = false;
        this.workflowResource = plmbrWorkflowResource;
        // no need to get tasks on cancel - component is about to be destroyed.
        this.onActioned = $rootScope.$on(constants_1.constants.events.workflowActioned, (_, data) => data.action != constants_1.constants.actions.cancel ? this.fetchTasks() : {});
        localizationService.localizeMany(['workflow_admin', 'workflow_stage', 'workflow_asAdmin', 'workflow_for', 'workflow_awaitingResubmission', 'workflow_pendingApproval', 'workflow_noGroup'])
            .then((resp) => {
            [this.adminStr, this.stageStr, this.asAdminStr, this.forStr, this.awaitingResubmissionStr, this.pendingApprovalStr, this.noGroupStr] = resp;
        });
        $scope.$on('$destroy', () => {
            this.onActioned();
        });
    }
}
exports.TaskListComponent = {
    name: 'workflowTaskList',
    transclude: true,
    template:'<umb-box class="workflow"><umb-box-header title-key="workflow_workflowActivity"></umb-box-header><umb-box-content><ul class="task-list" ng-if="$ctrl.tasks.length"><li class="task-list-item" ng-class="{\'task-list-item--dummy\': taskCollection[0].futureTask, \'history-task-collection\': taskCollection.length > 1}" ng-repeat="taskCollection in $ctrl.tasks track by $index"><div class="history-task-number"><span class="counter">{{ $ctrl.stageStr }} {{ $index + 1 }}</span></div><div class="history-tasks" ng-repeat="t in taskCollection | orderBy: \'id\'"><div class="history-item"><div class="history-item__break pb-0"><div class="history-item__avatar"><umb-avatar size="xs" color="secondary" name="{{ ::t.avatarName }}"></umb-avatar></div><div><div ng-bind="::t.whodunnit"></div><div class="history-item__date" ng-bind="::t.completedDate" ng-if="t.completedDate"></div></div></div><div class="history-item__break pb-0"><umb-badge color="{{ ::t.statusColor }}" size="xs" ng-bind="::t.statusName"></umb-badge></div></div><div ng-if="t.comment && t.status !== 4" class="history-item__comment"><umb-icon icon="icon-quote"></umb-icon><p class="comment-text" ng-bind="::t.comment"></p></div></div></li></ul><umb-load-indicator ng-show="!$ctrl.tasksLoaded"></umb-load-indicator></umb-box-content></umb-box>',
    bindings: {
        instanceGuid: '<',
        permissions: '<'
    },
    controller: WorkflowTaskList
};

},{"../../js/constants":22}],21:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TasksComponent = void 0;
const models_1 = require("../../js/models");
const tasklist_base_1 = require("../../js/models/tasklist.base");
class WorkflowTasks extends tasklist_base_1.TaskListBase {
    constructor($scope, $rootScope, notificationsService, plmbrActionsService) {
        super($scope, $rootScope, notificationsService, plmbrActionsService);
        this.pagination = new models_1.Pagination(() => this.fetch(), 5);
        this.sorter = new models_1.Sorter(() => this.fetch());
        this.sort = (key) => this.sorter.update(key);
    }
    $onChanges(change) {
        if (change.model.currentValue) {
            this.model = change.model.currentValue;
            this.pagination.perPage = this.model.perPage || this.pagination.perPage;
            this.fetch();
        }
    }
    fetch() {
        var _a, _b;
        // only submit once as angular events may fire multiple times
        if (this.paging)
            return;
        const query = {
            userId: this.model.userId,
            groupId: this.model.groupId,
            page: this.pagination.pageNumber - 1,
            count: this.pagination.perPage,
            sortBy: this.sorter.sortBy,
            sortDirection: this.sorter.sortDirectionString,
        };
        if ((_a = this.model.filters) === null || _a === void 0 ? void 0 : _a.keys)
            query.filters = this.model.filters.keys;
        if ((_b = this.model.filters) === null || _b === void 0 ? void 0 : _b.values)
            query.filterValues = this.model.filters.values;
        this.doFetch(query, this.pagination);
    }
    ;
}
exports.TasksComponent = {
    name: 'workflowTasks',
    transclude: true,
    bindings: {
        model: '<'
    },
    template:'<div class="relative workflow-tasks"><div ng-show="$ctrl.items.length && !$ctrl.paging"><div class="umb-table"><div class="umb-table-head"><div class="umb-table-row"><div class="umb-table-cell umb-table__name not-fixed"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'nodeName\')"><localize key="headers_page">Page</localize><span ng-if="$ctrl.languageCount > 1">&nbsp;(<localize key="general_language">Language</localize>)</span><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'nodeName\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'nodeName\'] }}"></umb-icon></button></div><div class="umb-table-cell"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'type\')"><localize key="content_type">Type</localize><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'type\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'type\'] }}"></umb-icon></button></div><div class="umb-table-cell hide-sm" ng-if="::$ctrl.model.type === 0"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'requestedBy\')"><localize key="workflow_requestedBy">Requested by</localize><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'requestedBy\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'requestedBy\'] }}"></umb-icon></button></div><div class="umb-table-cell hide-sm"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'id\')"><localize key="workflow_requestedOn">Requested on</localize><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'id\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'id\'] }}"></umb-icon></button></div><div class="umb-table-cell"><button type="button" class="umb-table-head__link sortable" ng-click="$ctrl.sort(\'comment\')"><localize key="general_comment">Comment</localize><umb-icon class="umb-table-head__icon" ng-if="$ctrl.sorter.sortBy === \'comment\'" icon="icon-navigation-{{ $ctrl.sorter.sortDirections[\'comment\'] }}"></umb-icon></button></div><div class="umb-table-cell umb-table-cell__detail"></div></div></div><div class="umb-table-body"><div class="table-row-outer" ng-repeat="item in $ctrl.items track by $index"><div class="umb-table-row"><div class="umb-table-cell umb-table__name not-fixed"><a href="#" ng-href="#/content/content/edit/{{item.node.id}}?cculture={{$ctrl.variantCode(item)}}">{{ $ctrl.displayName(item) }}</a></div><div class="umb-table-cell" ng-bind="item.type"></div><div class="umb-table-cell hide-sm" ng-bind="item.requestedBy" ng-if="$ctrl.model.type === 0"></div><div class="umb-table-cell hide-sm" ng-bind="item.requestedOn"></div><div class="umb-table-cell" ng-bind="item.comment"></div><div class="umb-table-cell umb-table-cell__detail show-overflow"><umb-button action="$ctrl.detail(item)" type="button" button-style="info" add-ellipsis="true" state="$ctrl.buttonStates[item.instanceGuid]" label-key="workflow_detail"></umb-button></div></div><div class="workflow-progress-container" ng-if="item.tasks"><workflow-progress instance="item"></workflow-progress></div></div></div></div></div><div class="flex justify-center"><umb-pagination ng-if="$ctrl.pagination.totalPages > 1 && $ctrl.items.length" page-number="$ctrl.pagination.pageNumber" total-pages="$ctrl.pagination.totalPages" on-next="$ctrl.pagination.goToPage" on-prev="$ctrl.pagination.goToPage" on-go-to-page="$ctrl.pagination.goToPage"></umb-pagination></div><umb-empty-state ng-if="!$ctrl.items.length && $ctrl.loaded"><localize key="content_listViewNoItems">There are no items show in the list.</localize></umb-empty-state><umb-load-indicator ng-if="!$ctrl.loaded || $ctrl.paging"></umb-load-indicator></div>',
    controller: WorkflowTasks
};

},{"../../js/models":29,"../../js/models/tasklist.base":33}],22:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.constants = void 0;
exports.constants = {
    workflow: 'workflow',
    events: {
        workflowAction: 'workflowAction',
        workflowActioned: 'workflowActioned',
        workflowDetail: 'workflowDetail',
        workflowAppActive: 'workflowAppActive',
        buttonStateChanged: 'buttonStateChanged',
        configSaved: 'configSaved',
        contentSaved: 'content.saved',
        contentUnpublished: 'content.unpublished',
        appTabChange: 'app.tabChange',
        editorsOpen: 'appState.editors.open',
        editorClose: 'appState.editors.close',
        refreshGroups: 'refreshGroups',
        goToNode: 'goToNode',
    },
    actions: {
        publish: 'publish',
        unpublish: 'unpublish',
        initiate: 'initiate',
        cancel: 'cancel',
        reject: 'reject',
        save: 'save',
        saveAndPublish: 'saveAndPublish',
        schedulePublish: 'schedulePublish',
        publishDescendant: 'publishDescendant',
        sendToPublish: 'sendToPublish',
    },
    sizes: {
        s: 'small',
        m: 'medium',
        l: 'large',
    },
    states: {
        success: 'success',
        error: 'error',
        busy: 'busy',
        init: 'init',
        rejected: 'rejected',
        detail: 'detail',
        published: 'published',
        draft: 'draft',
    },
    updatePromptKey: 'plumberUpdatePrompt',
    invariantCulture: '*',
};

},{}],23:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.DirectivesModule = void 0;
const impersonationbanner_directive_1 = require("./impersonationbanner.directive");
const license_directive_1 = require("./license.directive");
const umblistview_directive_1 = require("./umblistview.directive");
const umbtablerow_directive_1 = require("./umbtablerow.directive");
exports.DirectivesModule = angular
    .module('plumber.directives', [])
    .directive(impersonationbanner_directive_1.ImpersonationBanner.directiveName, ($compile) => new impersonationbanner_directive_1.ImpersonationBanner($compile))
    .directive(license_directive_1.License.directiveName, (plmbrWorkflowResource, localizationService) => new license_directive_1.License(plmbrWorkflowResource, localizationService))
    .directive(umblistview_directive_1.UmbListView.directiveName, (plmbrWorkflowResource) => new umblistview_directive_1.UmbListView(plmbrWorkflowResource))
    .directive(umbtablerow_directive_1.UmbTableRow.directiveName, () => new umbtablerow_directive_1.UmbTableRow())
    .name;

},{"./impersonationbanner.directive":24,"./license.directive":25,"./umblistview.directive":26,"./umbtablerow.directive":27}],24:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ImpersonationBanner = void 0;
class ImpersonationBanner {
    constructor($compile) {
        this.$compile = $compile;
        this.restrict = 'A';
    }
    link(scope, element) {
        const license = Umbraco.Sys.ServerVariables.Plumber.license;
        if (license.isImpersonating) {
            const template = ` 
                <div class="alert alert-workflow">
                    <umb-icon icon="icon-alert"></umb-icon>
                    <p>Plumber license impersonation is active. All features are available on non-production domains only.</p>
                </div>`;
            element.prepend(this.$compile(template)(scope));
        }
    }
}
exports.ImpersonationBanner = ImpersonationBanner;
ImpersonationBanner.directiveName = 'impersonationBanner';

},{}],25:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.License = void 0;
class License {
    constructor(plmbrWorkflowResource, localizationService) {
        this.plmbrWorkflowResource = plmbrWorkflowResource;
        this.localizationService = localizationService;
        this.restrict = 'A';
    }
    link(scope, element, attrs) {
        const bind = () => {
            // if the value is empty or false, return
            if (attrs.licensed === 'false') {
                return;
            }
            const license = Umbraco.Sys.ServerVariables.Plumber.license;
            let title, content;
            this.localizationService.localizeMany(['workflow_licensedFeature', 'workflow_licensedFeatureDescription'])
                .then(result => {
                [title, content] = result;
                document.body.style.setProperty('--plumberUnlicensed', `'${title}'`);
            });
            if (!license || license.isTrial && !license.isImpersonating) {
                element[0].classList.add('unlicensed');
                element[0].addEventListener('click', () => {
                    this.plmbrWorkflowResource.htmlOverlay(title, content);
                }, false);
                // this is a bit hacky, but needs the timeout to ensure the property-editor is
                // full rendered before looking for the deep-nested buttons
                setTimeout(() => {
                    const buttons = element[0].querySelectorAll('button');
                    if (buttons.length) {
                        for (let i = 0; i < buttons.length; i += 1) {
                            buttons[i].tabIndex = -1;
                        }
                    }
                }, 500);
            }
        };
        scope.$on('licenseCheck', () => bind());
        bind();
    }
}
exports.License = License;
License.directiveName = 'licensed';

},{}],26:[function(require,module,exports){
"use strict";
// directives here are used to change the icon on nodes in a list view
// fetches the status for all nodes in the current view
// sets a class on the list view row if an active workflow exists
// raises an event once that is complete
// in the umbtablerow directive, the event triggers adding a class to the table row which changes the icon and title attribute
Object.defineProperty(exports, "__esModule", { value: true });
exports.UmbListView = void 0;
class UmbListView {
    constructor(plmbrWorkflowResource) {
        this.restrict = 'C';
        this.workflowResource = plmbrWorkflowResource;
    }
    link(scope) {
        const setIcons = nodes => scope.listViewResultSet.items.forEach(v => v.activeWorkflow = nodes[v.id] && nodes[v.id] === true);
        scope.$watch(() => scope.listViewResultSet.items, (a, b) => {
            if (a && a.length && a !== b) {
                scope.items = a;
                scope.ids = scope.items.map(i => i.id);
                this.workflowResource.getStatus(scope.ids.join(','))
                    .then(resp => {
                    setIcons(resp.nodes);
                    scope.$broadcast('listViewStatus');
                });
            }
        });
    }
}
exports.UmbListView = UmbListView;
UmbListView.directiveName = 'umbListview';

},{}],27:[function(require,module,exports){
"use strict";
// directives here are used to change the icon on nodes in a list view
// fetches the status for all nodes in the current view
// sets a class on the list view row if an active workflow exists
// raises an event once that is complete
// in the table row directive, the event triggers adding a class to the table row which changes the icon and title attribute
Object.defineProperty(exports, "__esModule", { value: true });
exports.UmbTableRow = void 0;
class UmbTableRow {
    constructor() {
        this.restrict = 'C';
    }
    link(scope, element) {
        scope.$on('listViewStatus', () => {
            if (scope.item && scope.item.activeWorkflow) {
                element[0].classList.add('active-workflow');
                element[0].childNodes.forEach(c => {
                    if (c.classList && c.classList.contains('umb-table-cell')) {
                        c.setAttribute('title', 'Workflow active');
                    }
                });
            }
        });
    }
}
exports.UmbTableRow = UmbTableRow;
UmbTableRow.directiveName = 'umbTableRow';

},{}],28:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WorkflowStatus = void 0;
var WorkflowStatus;
(function (WorkflowStatus) {
    WorkflowStatus[WorkflowStatus["Approved"] = 1] = "Approved";
    WorkflowStatus[WorkflowStatus["Rejected"] = 2] = "Rejected";
    WorkflowStatus[WorkflowStatus["PendingApproval"] = 3] = "PendingApproval";
    WorkflowStatus[WorkflowStatus["NotRequired"] = 4] = "NotRequired";
    WorkflowStatus[WorkflowStatus["Cancelled"] = 5] = "Cancelled";
    WorkflowStatus[WorkflowStatus["Errored"] = 6] = "Errored";
    WorkflowStatus[WorkflowStatus["Resubmitted"] = 7] = "Resubmitted";
    WorkflowStatus[WorkflowStatus["CancelledByThirdParty"] = 8] = "CancelledByThirdParty";
})(WorkflowStatus = exports.WorkflowStatus || (exports.WorkflowStatus = {}));

},{}],29:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SortOptions = exports.Sorter = exports.Pagination = void 0;
var pagination_1 = require("./pagination");
Object.defineProperty(exports, "Pagination", { enumerable: true, get: function () { return pagination_1.Pagination; } });
var sorter_1 = require("./sorter");
Object.defineProperty(exports, "Sorter", { enumerable: true, get: function () { return sorter_1.Sorter; } });
var sortoptions_1 = require("./sortoptions");
Object.defineProperty(exports, "SortOptions", { enumerable: true, get: function () { return sortoptions_1.SortOptions; } });

},{"./pagination":30,"./sorter":31,"./sortoptions":32}],30:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.Pagination = void 0;
/**
* Defaults to pageNumber = 1, totalPages = 0, perPage = 10
* Pass a function to the constructor to use as the goToPage callback
* goToPage manages setting the page number before executing the callback
* */
class Pagination {
    constructor(cb, perPage = 10) {
        this.pageNumber = 1;
        this.totalPages = 0;
        this.perPage = perPage;
        this.goToPage = i => {
            this.pageNumber = i;
            cb();
        };
    }
}
exports.Pagination = Pagination;

},{}],31:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SortDirection = exports.Sorter = void 0;
class Sorter {
    constructor(callback, sortBy = 'id', sortDirection = SortDirection.ASC) {
        this.sortDirections = {};
        this.sortBy = sortBy;
        this.sortDirections[this.sortBy] = sortDirection;
        this.callback = callback;
    }
    setDirection(sortDirection) {
        this.sortDirections[this.sortBy] = sortDirection;
    }
    update(sortBy) {
        this.sortBy = sortBy;
        this.sortDirections[sortBy] = this.sortDirections[sortBy] === SortDirection.ASC
            ? SortDirection.DESC
            : SortDirection.ASC;
        this.callback();
    }
    get sortDirectionString() {
        return this.sortDirections[this.sortBy] === SortDirection.ASC ? 'asc' : 'desc';
    }
}
exports.Sorter = Sorter;
var SortDirection;
(function (SortDirection) {
    SortDirection["ASC"] = "up";
    SortDirection["DESC"] = "down";
})(SortDirection = exports.SortDirection || (exports.SortDirection = {}));

},{}],32:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.SortOptions = void 0;
class SortOptions {
    constructor(stop) {
        this.axis = 'y';
        this.containment = 'parent';
        this.distance = 10;
        this.opacity = 0.7;
        this.tolerance = 'pointer';
        this.scroll = true;
        this.zIndex = 6000;
        this.stop = () => { };
        this.stop = () => {
            stop ? stop() : {};
        };
    }
}
exports.SortOptions = SortOptions;

},{}],33:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.TaskListBase = void 0;
const constants_1 = require("../constants");
class TaskListBase {
    constructor($scope, $rootScope, notificationsService, plmbrActionsService) {
        this.$rootScope = $rootScope;
        this.notificationsService = notificationsService;
        this.plmbrActionsService = plmbrActionsService;
        this.buttonStates = [];
        this.notificationsService = notificationsService;
        this.actionsService = plmbrActionsService;
        this.onDetail = $rootScope.$on(constants_1.constants.events.workflowDetail, (_, data) => this.setButtonState(data));
        this.languageCount = Umbraco.Sys.ServerVariables.Plumber.languageCount;
        $scope.$on('$destroy', () => {
            this.onDetail();
        });
    }
    doFetch(query, pagination) {
        this.paging = true;
        this.model.handler(query)
            .then(resp => {
            this.items = resp.items;
            pagination.totalPages = resp.totalPages;
        }, error => this.notificationsService.error('Error', error.message))
            .finally(() => {
            this.loaded = true;
            this.paging = false;
        });
    }
    displayName(item) {
        return this.languageCount > 1 ? `${item.node.name} (${item.variantName})` : item.node.name;
    }
    detail(item) {
        this.buttonStates[item.instanceGuid] = constants_1.constants.states.busy;
        this.actionsService.detail(item);
    }
    setButtonState(data) {
        this.buttonStates[data.instanceGuid] = constants_1.constants.states.init;
    }
    variantCode(item) {
        return item.variantCode === '*' ? Umbraco.Sys.ServerVariables.Plumber.defaultCulture : item.variantCode;
    }
}
exports.TaskListBase = TaskListBase;

},{"../constants":22}],34:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ServicesModule = void 0;
const actions_resource_1 = require("./actions.resource");
const groups_resource_1 = require("./groups.resource");
const hub_resource_1 = require("./hub.resource");
const state_factory_1 = require("./state.factory");
const licensing_resource_1 = require("./licensing.resource");
const workflow_resource_1 = require("./workflow.resource");
exports.ServicesModule = angular
    .module('plumber.services', [])
    .service(actions_resource_1.ActionsService.serviceName, actions_resource_1.ActionsService)
    .service(groups_resource_1.GroupsService.serviceName, groups_resource_1.GroupsService)
    .service(hub_resource_1.PlumberHub.serviceName, hub_resource_1.PlumberHub)
    .service(state_factory_1.StateFactory.serviceName, state_factory_1.StateFactory)
    .service(licensing_resource_1.LicensingService.serviceName, licensing_resource_1.LicensingService)
    .service(workflow_resource_1.WorkflowService.serviceName, workflow_resource_1.WorkflowService)
    .name;

},{"./actions.resource":35,"./groups.resource":36,"./hub.resource":37,"./licensing.resource":38,"./state.factory":40,"./workflow.resource":41}],35:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.ActionsService = void 0;
const constants_1 = require("../constants");
const enums_1 = require("../models/enums");
class ActionsService {
    constructor($rootScope, editorService, $timeout, localizationService, navigationService, $routeParams, plmbrWorkflowResource, plmbrStateFactory) {
        this.$rootScope = $rootScope;
        this.editorService = editorService;
        this.$timeout = $timeout;
        this.localizationService = localizationService;
        this.navigationService = navigationService;
        this.$routeParams = $routeParams;
        /**
         * UI feedback for button directive
         * @param {any} state
         * @param {any} id
         */
        this.buttonState = (state, id) => this.$rootScope.$emit(constants_1.constants.events.buttonStateChanged, { state: state, id: id });
        /**
         * display notification after actioning workflow task
         * @param {any} d
         * @param {any} item
         * @param {any} action
         */
        this.notify = (d, item, action) => {
            var _a;
            this.buttonState(d.status === 200 ? constants_1.constants.states.success : constants_1.constants.states.error, item.node.id);
            const isScheduled = typeof item.scheduledDate !== 'undefined';
            const type = item.typeId === 1 ? constants_1.constants.actions.publish : item.typeId === 2 ? constants_1.constants.actions.unpublish : null;
            this.$rootScope.$emit(constants_1.constants.events.workflowActioned, {
                action,
                nodeId: item.node.id,
                variant: item.variant,
                isScheduled,
                type,
                status: d.workflowStatus
            });
            // sync the content tree to ensure current node state is correct, only after a workflow is complete
            // as any other state won't have modified the node state, so no need to refresh
            if (this.workflowStateFactory.path && d.workflowStatus.includes(enums_1.WorkflowStatus.Approved) && ((_a = this.$routeParams) === null || _a === void 0 ? void 0 : _a.tree)) {
                this.navigationService.syncTree({ tree: this.$routeParams.tree, path: this.workflowStateFactory.path.split(',').map(x => +x), forceReload: true });
            }
            // finally, close the overlay if this is a dashboard action
            if (this.workflowOverlay.close) {
                this.workflowOverlay.close();
            }
        };
        /**
         *
         * @param {any} item
         * @param {any} comment
         * @param {any} action
         * @param {any} offline
         */
        this.action = (item, comment, action, offline) => {
            this.buttonState(constants_1.constants.states.busy, item.node.id);
            this.workflowResource.actionWorkflow({
                action,
                instanceGuid: item.instance.guid,
                comment,
                offline
            }).then(resp => this.notify(resp, item, action));
        };
        /**
         *
         * @param {InitiateWorkflowModel} args
         */
        this.initiate = (model) => {
            // notify expects an object
            const item = {
                node: { id: model.nodeId },
                typeId: model.publish ? 1 : 2,
                variant: model.variant,
            };
            if (model.scheduledDate) {
                item.scheduledDate = model.scheduledDate;
            }
            this.buttonState(constants_1.constants.states.busy, model.nodeId);
            this.workflowResource.initiateWorkflow(model)
                .then(resp => this.notify(resp, item, constants_1.constants.actions.initiate));
        };
        /**
         *
         * @param {any} instance
         */
        this.detail = (instance) => {
            this.localizationService.localizeMany(['workflow_pendingForNode', 'workflow_historyFor'])
                .then(resp => this.doDetailOverlay(instance, resp[0], resp[1]));
        };
        /**
         *
         * @param {any} instance
         * @param {any} pendingForNode
         * @param {any} historyFor
         */
        this.doDetailOverlay = (instance, pendingForNode, historyFor) => {
            const viewsPath = Umbraco.Sys.ServerVariables.Plumber.viewsPath;
            pendingForNode = pendingForNode.replace('%0%', instance.type.toLowerCase());
            pendingForNode = pendingForNode.replace('%1%', instance.node.name);
            // readonly when the workflow is non-active
            const readonly = instance.completedDate != null;
            this.workflowOverlay = {
                view: `${viewsPath}overlays/workflow.detail.overlay.html`,
                size: readonly ? constants_1.constants.sizes.m : constants_1.constants.sizes.l,
                title: pendingForNode,
                description: instance.typeDescription,
                instance,
                readonly,
                close: () => this.editorService.close()
            };
            if (instance.node.url !== '#') {
                this.workflowOverlay.description = instance.node.url;
            }
            if (readonly || !instance.node.exists) {
                this.workflowOverlay.title = historyFor.replace('%0%', instance.node.name);
                this.editorService.open(this.workflowOverlay);
            }
            else {
                const content = {
                    id: instance.node.id,
                    path: instance.path,
                    contentTypeId: instance.contentTypeId
                };
                // if not readonly, we need state to allow correct action buttons
                this.workflowStateFactory.getButtonState({ content, variantCode: instance.variantCode }).then(state => {
                    this.workflowOverlay.state = state;
                    this.$timeout(() => {
                        this.editorService.open(this.workflowOverlay);
                        this.$rootScope.$emit(constants_1.constants.events.workflowDetail, { instanceGuid: instance.instanceGuid });
                    });
                });
            }
        };
        /**
         * Offline gets state in the controller to check the user group is permitted to action, hence the simpler method here
         */
        this.offlineDetail = (instance, state) => {
            this.localizationService.localize('workflow_pendingForNode')
                .then(pendingForNode => this.doOfflineDetail(instance, state, pendingForNode));
        };
        /**
         *
         * @param {any} instance
         * @param {any} state
         * @param {any} pendingForNode
         */
        this.doOfflineDetail = (instance, state, pendingForNode) => {
            const viewsPath = Umbraco.Sys.ServerVariables.Plumber.viewsPath;
            pendingForNode = pendingForNode.replace('%0%', instance.type.toLowerCase());
            pendingForNode = pendingForNode.replace('%1%', instance.node.name);
            this.workflowOverlay = {
                view: `${viewsPath}overlays/workflow.detail.overlay.html`,
                size: constants_1.constants.sizes.l,
                title: pendingForNode,
                instance: instance,
                readonly: false,
                state: state,
                close: () => this.editorService.close()
            };
            if (instance.node.url !== '#') {
                this.workflowOverlay.description = instance.node.url;
            }
            this.editorService.open(this.workflowOverlay);
            this.$rootScope.$emit(constants_1.constants.events.workflowDetail, { instanceGuid: instance.instanceGuid });
        };
        this.workflowResource = plmbrWorkflowResource;
        this.workflowStateFactory = plmbrStateFactory;
        this.workflowOverlay = {};
        $rootScope.$on(constants_1.constants.events.goToNode, () => this.workflowOverlay.close());
    }
}
exports.ActionsService = ActionsService;
ActionsService.serviceName = 'plmbrActionsService';

},{"../constants":22,"../models/enums":28}],36:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.GroupsService = void 0;
const constants_1 = require("../constants");
class GroupsService {
    constructor($http, $rootScope, umbRequestHelper, editorService) {
        this.$http = $http;
        this.umbRequestHelper = umbRequestHelper;
        this.editorService = editorService;
        this.request = (method, url, data) => {
            url = `${Umbraco.Sys.ServerVariables.Plumber.groupsApiBaseUrl}${url}`;
            return this.umbRequestHelper.resourcePromise(method === 'DELETE' ? this.$http.delete(url)
                : method === 'POST' ? this.$http.post(url, data)
                    : method === 'PUT' ? this.$http.put(url, data)
                        : this.$http.get(url), 'Something broke');
        };
        /**
         * @returns {array} user groups
         * @description Get single group by id
         */
        this.get = (id) => this.request('GET', `Get?id=${id}`);
        /**
         * @returns {array} user group pocos - userId, groupId, inherited
         * @param {Array<number|string>} ids array of Umbraco group ids to query
         */
        this.getInheritedMembers = (ids) => this.request('GET', `GetInheritedMembers?ids=${ids.join(',')}`);
        /**
         * @param {number} page defaults to 0
         * @param {number} count defaults to 10000
         * @param {string} filter defaults to ''
         * @param {boolean} slim defaults to false
         */
        this.getPage = (page, count, filter = '', slim = false) => this.request('GET', `GetPage?slim=${slim}&page=${page}&count=${count}&filter=${filter}`);
        /**
         * @returns {array} user groups
         * @description Get an array of slim user groups (id, name only)
         */
        this.getAllSlim = () => this.request('GET', 'GetPage?slim=true&page=1&count=10000');
        /**
         * Get the bare minimum content info - path, id, trashed, icon, name
         * @param {any} ids
         */
        this.getContentSlim = ids => this.request('GET', `GetContentSlim?ids=${ids.join(',')}`);
        /**
         * @param {object} group
         * @returns {string}
         * @description save updates to an existing group object
         */
        this.save = group => this.request('PUT', 'Save', group);
        /**
         * @param {number} id
         * @returns {string}
         * @description delete group by id
         */
        this.delete = (id) => this.request('DELETE', 'Delete/' + id);
        /**
         *
         * @param group
         */
        this.editGroup = (groupId) => {
            const model = {
                view: Umbraco.Sys.ServerVariables.Plumber.pluginPath + '/approval-groups/edit.html',
                size: constants_1.constants.sizes.l,
                groupId: groupId,
                submit: _ => {
                    this.editorService.close();
                },
                close: () => this.editorService.close(),
            };
            this.editorService.open(model);
        };
        /**
         *
         * @param name
         * @param index
         */
        this.generateNameWithStage = (name, index) => `Stage ${index + 1}: ${name}`;
        $rootScope.$on(constants_1.constants.events.refreshGroups, () => this.getAllSlim());
    }
}
exports.GroupsService = GroupsService;
GroupsService.serviceName = 'plmbrGroupsResource';

},{"../constants":22}],37:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PlumberHub = void 0;
class PlumberHub {
    constructor($rootScope, $q, assetsService) {
        this.$rootScope = $rootScope;
        this.$q = $q;
        this.assetsService = assetsService;
        this.scripts = [];
        this.starting = false;
        this.callbacks = [];
        this.setupHub = callback => {
            let proxy;
            let hub = {};
            $.connection = new signalR.HubConnectionBuilder()
                .withUrl(Umbraco.Sys.ServerVariables.Plumber.signalRHub, {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets,
            })
                .withAutomaticReconnect()
                .configureLogging(signalR.LogLevel.Warning)
                .build();
            proxy = $.connection;
            if (proxy !== undefined) {
                hub = {
                    active: true,
                    start: () => {
                        try {
                            proxy.start()
                                .then(() => { } /*console.info('Hub started =>', $.connection.connectionId)*/)
                                .catch(() => console.warn('Failed to start hub'));
                        }
                        catch (e) {
                            console.warn('Could not setup signalR connection', e);
                        }
                    },
                    on: (eventName, callback) => {
                        proxy.on(eventName, result => {
                            this.$rootScope.$apply(() => {
                                if (callback) {
                                    callback(result);
                                }
                            });
                        });
                    },
                    /**
                     * Function is common across 472 and 5.0
                     * @param methodName
                     * @param callback
                     */
                    invoke: (methodName, callback) => {
                        proxy.invoke(methodName)
                            .done(result => this.$rootScope.$apply(() => {
                            if (callback) {
                                callback(result);
                            }
                        }));
                    }
                };
            }
            else {
                hub = {
                    on: () => { },
                    invoke: () => { },
                    start: () => console.warn('No hub to start'),
                };
            }
            return callback(hub);
        };
        const umbracoPath = Umbraco.Sys.ServerVariables.umbracoSettings.umbracoPath;
        this.scripts = [umbracoPath + '/lib/signalr/signalr.min.js'];
    }
    /**
     * Function is common across 472 and 5.0
     * */
    processCallbacks() {
        while (this.callbacks.length) {
            const cb = this.callbacks.pop();
            this.setupHub(cb);
        }
        this.starting = false;
    }
    /**
     * Function is common across 472 and 5.0
     * @param callback
     */
    initHub(callback) {
        this.callbacks.push(callback);
        if (!this.starting) {
            if ($.connection === undefined) {
                this.starting = true;
                const promises = [];
                this.scripts.forEach(script => promises.push(this.assetsService.loadJs(script)));
                this.$q.all(promises).then(() => this.processCallbacks());
            }
            else {
                this.processCallbacks();
            }
        }
    }
}
exports.PlumberHub = PlumberHub;
PlumberHub.serviceName = 'plumberHub';

},{}],38:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.LicensingService = void 0;
class LicensingService {
    constructor($http, umbRequestHelper) {
        this.$http = $http;
        this.umbRequestHelper = umbRequestHelper;
        this.request = (method, url, data) => {
            url = `${Umbraco.Sys.ServerVariables.Plumber.licensingApiBaseUrl}${url}`;
            return this.umbRequestHelper.resourcePromise(method === 'GET'
                ? this.$http.get(url)
                : this.$http.post(url, data), 'Something broke');
        };
        this.validate = (license, key) => this.request('POST', 'Validate', { license, key });
        this.getEula = () => this.request('GET', 'GetEula');
        this.getLicensingUrl = () => this.request('GET', 'GetLicensingUrl');
    }
}
exports.LicensingService = LicensingService;
LicensingService.serviceName = 'plmbrLicensingResource';

},{}],39:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.PlumberServiceBase = void 0;
class PlumberServiceBase {
    constructor($http, $routeParams, umbRequestHelper, navigationService) {
        this.$http = $http;
        this.$routeParams = $routeParams;
        this.umbRequestHelper = umbRequestHelper;
        this.navigationService = navigationService;
        /**
         *
         * @param {any} method
         * @param {any} url
         * @param {any} data
         */
        this.request = (method, url, data) => this.umbRequestHelper.resourcePromise(method === 'DELETE' ? this.$http.delete(url)
            : method === 'POST' ? this.$http.post(url, data)
                : method === 'PUT' ? this.$http.put(url, data)
                    : this.$http.get(url), 'Something broke');
        /**
         * sync the tree in the workflow section
         */
        this.setTreeState = () => {
            if (this.$routeParams.section === 'workflow') {
                this.navigationService.syncTree({ tree: this.$routeParams.tree, path: [-1], forceReload: false });
            }
        };
        this.urls = {
            settings: Umbraco.Sys.ServerVariables.Plumber.settingsApiBaseUrl,
            tasks: Umbraco.Sys.ServerVariables.Plumber.tasksApiBaseUrl,
            instances: Umbraco.Sys.ServerVariables.Plumber.instancesApiBaseUrl,
            actions: Umbraco.Sys.ServerVariables.Plumber.actionsApiBaseUrl,
            config: Umbraco.Sys.ServerVariables.Plumber.configApiBaseUrl,
            scaffold: Umbraco.Sys.ServerVariables.Plumber.scaffoldApiBaseUrl
        };
    }
}
exports.PlumberServiceBase = PlumberServiceBase;

},{}],40:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.StateFactory = void 0;
const constants_1 = require("../constants");
class StateFactory {
    constructor($rootScope, $location, plmbrWorkflowResource) {
        this.$rootScope = $rootScope;
        this.$location = $location;
        this.rejected = false;
        this.canAction = false;
        this.canResubmit = false;
        this.canEdit = true;
        this.isChangeAuthor = false;
        this.isAdmin = false;
        ///
        this.setNodeState = (currentTask) => {
            var _a, _b;
            this.rejected = false;
            this.isChangeAuthor = false;
            this.canAction = false;
            this.canEdit = true;
            if (currentTask) {
                this.rejected = currentTask.cssStatus === constants_1.constants.states.rejected;
                // if the task has been rejected and the current user requested the change, let them edit
                this.isChangeAuthor = currentTask.requestedById === this.user.id;
                // if the current user is a member of the group and task is pending, they can action, UNLESS...
                // if the user requested the change, is a member of the current group, and flow type is exclude, they cannot action
                this.canAction = ((_b = (_a = currentTask.userGroup) === null || _a === void 0 ? void 0 : _a.usersSummary) === null || _b === void 0 ? void 0 : _b.indexOf(`|${this.user.id}|`)) !== -1 && !this.rejected;
                if (this.settings.flowType === 2 && this.isChangeAuthor && this.canAction) {
                    this.canAction = false;
                }
                this.canResubmit = this.rejected && this.isChangeAuthor;
                this.canEdit = this.rejected && this.isChangeAuthor || this.isAdmin;
            }
        };
        /**
         * Trial license must have node or inherited config and !excluded for config to be valid
         * Other license types can have any of node, contentType or inherited, and !excluded
         */
        this.hasValidConfig = () => {
            if (this.config.excluded) {
                return false;
            }
            if (Umbraco.Sys.ServerVariables.Plumber.license.isTrial) {
                return this.config.node.length
                    || this.config.inherited.length
                    || this.config.new.length;
            }
            return this.config.node.length
                || this.config.contentType.length
                || this.config.new.length
                || this.config.inherited.length;
        };
        /**
         * Remove buttons by alias from the provided button set
         * @param buttons
         * @param keys
         */
        this.removeButtons = (buttons, keys) => {
            keys.forEach(key => {
                const idx = buttons.findIndex(x => x.alias === key);
                if (idx === -1)
                    return;
                buttons.splice(idx, 1);
            });
        };
        this.workflowResource = plmbrWorkflowResource;
        this.nodeId;
        this.buttons = {
            detail: {
                labelKey: 'workflow_detailButton',
                buttonStyle: constants_1.constants.states.detail,
                addEllipsis: true,
                handler: () => this.showApp(),
                shortcut: 'ctrl+d'
            },
            publish: {
                labelKey: 'workflow_publishButton',
                buttonStyle: constants_1.constants.states.success,
                addEllipsis: true,
                handler: () => this.showApp(constants_1.constants.actions.publish),
                shortcut: 'ctrl+p'
            },
            unpublish: {
                labelKey: 'workflow_unpublishButton',
                addEllipsis: true,
                handler: () => this.showApp(constants_1.constants.actions.unpublish),
                shortcut: 'ctrl+u'
            }
        };
        $rootScope.$on('$routeChangeStart', () => {
            this.originalDefaultButton = null;
            this.originalSubButtons = null;
        });
    }
    showApp(action) {
        // will have multiple apps when multiple infinite editors are open
        const apps = document.querySelectorAll('[data-element="sub-view-workflow"]');
        const tabScope = angular.element(apps[apps.length - 1]);
        if (tabScope) {
            if (action && this.nodeId !== 0) {
                this.$rootScope.$emit(constants_1.constants.events.workflowAppActive, { action, id: this.nodeId });
            }
            // this is a bit naive, but avoids scope lookup on DOM elements...
            tabScope['click']();
        }
    }
    /**
     * This isn't foolproof - if a node is loaded in an unpublished state, then published
     * the unpublish button won't display as it didn't exist in the original buttons when the
     * node first loaded. This appears to be the same behaviour in Umbraco without Plumber
     * @param subButtons
     * @param activeVariantState
     * @param currentTask
     */
    setUnpublishButtons(subButtons, activeVariantState, currentTask) {
        // check for wf unpublish before adding - it may have been cached as part of the set already
        const unpublishIndex = subButtons.findIndex(x => x.alias === constants_1.constants.actions.unpublish);
        const workflowUnpublishBtnIndex = subButtons.findIndex(x => x.alias === this.buttons.unpublish.alias);
        // only add unpublish if the node is unpublishable aka is published
        // this is variant-specific as each can be in a different state
        if (activeVariantState.toLowerCase().startsWith(constants_1.constants.states.published)) {
            // add buttons when unpublish exists (user has permission) and no workflow button added
            if (unpublishIndex !== -1 && workflowUnpublishBtnIndex === -1) {
                if (this.settings.requireUnpublish || this.hasUnpublishPermissions) {
                    // if unpublish is required, either add the button or replace the 
                    // umbraco unpublish, if the user has permission to unpublish
                    if (!this.isAdmin && !this.settings.extendPermissions) {
                        subButtons[unpublishIndex] = this.buttons.unpublish;
                    }
                    else {
                        subButtons.splice(unpublishIndex, 0, this.buttons.unpublish);
                    }
                }
            }
            else if (!currentTask) {
                // add the workflow unpublish button if settings dictate and it isn't already in the set
                if ((this.settings.requireUnpublish || this.hasUnpublishPermissions) && workflowUnpublishBtnIndex === -1) {
                    subButtons.push(this.buttons.unpublish);
                }
                // then add the original Umbraco unpublish if it exists in the original button set and not in the updated buttons
                const originalUnpublishIndex = this.originalSubButtons.findIndex(x => x.alias === constants_1.constants.actions.unpublish);
                if (originalUnpublishIndex > -1 && unpublishIndex === -1) {
                    subButtons.splice(originalUnpublishIndex, 0, this.originalSubButtons[originalUnpublishIndex]);
                }
            }
        }
        else if (activeVariantState.toLowerCase() === constants_1.constants.states.draft) {
            // if not published, no need for unpublish buttons
            if (unpublishIndex !== -1) {
                subButtons.splice(unpublishIndex, 1);
            }
            if (workflowUnpublishBtnIndex !== -1) {
                subButtons.splice(workflowUnpublishBtnIndex, 1);
            }
        }
    }
    /**
     * Offline state is simpler as it doesn't need to set buttons
     * as we can assume plenty about the state purely by being offline
     * @param {any} arg
     */
    getOfflineState(arg) {
        this.nodeId = arg.content.id;
        let variant;
        // offline approvale must get variant directly from path 
        if (window.location.pathname.startsWith('/workflow-offline')) {
            let pathArray = window.location.pathname.split('/');
            variant = pathArray[3]; // /workflow-offline/nodeId/variant/etc/etc/etc
            variant = variant === '0' ? '*' : variant; // can't pass * in the url, so is set to 0
        }
        const scaffoldQueryModel = {
            nodeId: this.nodeId,
            contentTypeId: arg.content.contentTypeId,
            path: arg.content.path,
            variant,
            includeServerVariables: true,
        };
        return this.workflowResource.scaffold(scaffoldQueryModel)
            .then(resp => {
            this.user = resp.user;
            this.pending = resp.tasks;
            this.config = resp.config;
            this.settings = resp.settings;
            const currentTask = this.pending.task;
            const variantTasks = this.pending.variantTasks;
            let hasConfig;
            this.isAdmin = this.user.allowedSections.includes('workflow');
            if (this.hasValidConfig()) {
                hasConfig = true;
                this.setNodeState(currentTask);
                let config = this.configObject({ hasConfig, currentTask, variantTasks });
                config['offline'] = true;
                return config;
            }
            else {
                // should never be here for offline approval, since config must exist for the offline approval
                // request to have ever been generated
                return this.noConfigObject();
            }
        });
    }
    /**
     *
     * @param {any} arg
     */
    getButtonState(arg) {
        var _a;
        // remove the Umbraco buttons to avoid flashing the default while the updated versions are calculated
        // caches in the factory using the current node id, and only sets on first request for button state
        // to avoid overwriting with the workflow buttons when a workflow is completed/cancelled
        this.originalSubButtons = arg.content.id != this.nodeId ? arg.subButtons : this.originalSubButtons || arg.subButtons;
        this.originalDefaultButton = arg.content.id != this.nodeId ? arg.defaultButton : this.originalDefaultButton || arg.defaultButton;
        this.nodeId = arg.isNew === 'true' ? Umbraco.Sys.ServerVariables.Plumber.newNodeFlowId : arg.content.id;
        this.path = (_a = arg.content) === null || _a === void 0 ? void 0 : _a.path;
        let variant;
        // get variant from URL, or from default culture if only a single variant, else from the active variant language object
        variant = arg.variantCode || this.$location.search().cculture || this.$location.search().mculture ||
            (arg.content.variants.length === 1 ? Umbraco.Sys.ServerVariables.Plumber.defaultCulture : arg.content.variants.filter(v => v.active)[0].language.culture);
        arg.subButtons = [];
        arg.defaultButton = undefined;
        const scaffoldQueryModel = {
            nodeId: this.nodeId,
            contentTypeId: arg.content.contentTypeId,
            path: arg.content.path,
            variant,
        };
        return this.workflowResource.scaffold(scaffoldQueryModel)
            .then(resp => {
            this.user = resp.user;
            this.pending = resp.tasks;
            this.config = resp.config;
            this.settings = resp.settings;
            const currentTask = this.pending.task;
            const variantTasks = this.pending.variantTasks;
            let subButtons;
            this.isAdmin = this.user.allowedSections.includes('workflow');
            if (this.hasValidConfig()) {
                this.setNodeState(currentTask);
                // set sub-buttons, and push default button into this set
                subButtons = this.originalSubButtons || [];
                // only modify buttons when contentForm property exists 
                // when no content form, it's a dashboard, so no buttons
                if (arg.contentForm) {
                    if (this.pending.task || this.pending.variantTasks && !this.isAdmin) {
                        subButtons = [];
                    }
                    else if (this.originalDefaultButton && !subButtons.find(x => x.alias === constants_1.constants.actions.saveAndPublish)) {
                        subButtons.splice(0, 0, this.originalDefaultButton);
                    }
                    // always remove send to publish, as this causes confusion since workflow
                    // is doing much the same thing, but better :)
                    this.removeButtons(subButtons, [constants_1.constants.actions.sendToPublish]);
                }
                // if lockIfActive remove save, save+publish
                if (this.settings.lockIfActive && (this.pending.task || this.pending.variantTasks)) {
                    if (arg.contentForm) {
                        this.removeButtons(subButtons, [constants_1.constants.actions.save, constants_1.constants.actions.saveAndPublish]);
                    }
                    // if locked, no one can edit, admin or otherwise, unless they have canResubmit permission
                    // because rejecting but not allowing edits is pointless
                    this.canEdit = this.canResubmit || false;
                }
                // if unpublish workflow config exists, we can include the request unpublish button even if unpublish is not required
                // this has to happen here becuase the guard below will prevent the value updating when resetting state
                // after saving config changes, because that happens in the workflow app
                this.hasUnpublishPermissions = this.config.node.length && this.config.node[0].type === 0
                    || this.config.contentType.length && this.config.contentType[0].type === 0
                    || this.config.inherited.length && this.config.inherited[0].type === 0;
                // if sub-buttons contains unpublish, add the request unpublish button
                // if user is admin, add the button, if not, replace existing unpublish
                // unpublish does not exist, add it - all workflow users should be able to request unpublish
                // finally, none of this needs to happen if requireUnpublish is not true
                if (arg.contentForm) {
                    const activeVariant = arg.content.variants.find(v => v.active);
                    this.setUnpublishButtons(subButtons, activeVariant.state, currentTask);
                    // if user is not admin, remove the save+publish, schedule and publishDescendants button
                    if (!this.isAdmin && !this.settings.extendPermissions) {
                        this.removeButtons(subButtons, [
                            constants_1.constants.actions.saveAndPublish,
                            constants_1.constants.actions.schedulePublish,
                            constants_1.constants.actions.publishDescendant
                        ]);
                    }
                }
                arg.defaultButton = currentTask ? this.buttons.detail : this.buttons.publish;
                arg.subButtons = subButtons;
                return this.configObject({
                    currentTask,
                    variantTasks,
                    defaultButton: arg.defaultButton,
                    subButtons,
                });
            }
            else {
                arg.defaultButton = this.originalDefaultButton;
                arg.subButtons = this.originalSubButtons;
                return this.noConfigObject({ defaultButton: arg.defaultButton, subButtons: arg.subButtons });
            }
        });
    }
    /**
     *
     * @param {any} arg an object containing lots of stuff
     */
    configObject(arg) {
        return {
            nodeId: this.nodeId,
            hasConfig: true,
            exclude: this.config.excluded,
            permissions: {
                node: this.config.node,
                contentType: this.config.contentType,
                inherited: this.config.inherited,
                new: this.config.new,
                active: this.config.node.length ? 'node'
                    : this.config.contentType.length ? 'contentType'
                        : this.config.inherited.length ? 'inherited' : ''
            },
            allowAttachments: this.settings.allowAttachments,
            requireUnpublish: this.settings.requireUnpublish,
            hasUnpublishPermissions: this.hasUnpublishPermissions,
            isActive: !!arg.currentTask,
            isAdmin: this.isAdmin,
            canEdit: this.canEdit,
            currentTask: arg.currentTask,
            variantTasks: arg.variantTasks,
            canAction: this.canAction,
            rejected: this.rejected,
            canResubmit: this.canResubmit,
            isChangeAuthor: this.isChangeAuthor,
            userId: this.user.id,
            buttons: {
                defaultButton: arg.defaultButton,
                subButtons: arg.subButtons
            }
        };
    }
    noConfigObject(arg) {
        return {
            nodeId: this.nodeId,
            hasConfig: false,
            isAdmin: this.isAdmin,
            exclude: this.config.excluded,
            permissions: {
                node: [],
                contentType: [],
                inherited: [],
                new: this.config.new,
                active: this.config.new.length ? 'node' : '',
            },
            buttons: {
                defaultButton: arg.defaultButton,
                subButtons: arg.subButtons
            },
        };
    }
}
exports.StateFactory = StateFactory;
StateFactory.serviceName = 'plmbrStateFactory';

},{"../constants":22}],41:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.WorkflowService = void 0;
const filterpicker_1 = require("../../components/filterpicker/filterpicker");
const service_base_1 = require("./service-base");
class WorkflowService extends service_base_1.PlumberServiceBase {
    constructor($http, $routeParams, umbRequestHelper, navigationService, dateHelper, overlayService) {
        super($http, $routeParams, umbRequestHelper, navigationService);
        this.dateHelper = dateHelper;
        this.overlayService = overlayService;
        this.activityFilter = {};
        this.htmlOverlay = (title, content) => {
            const overlayModel = {
                view: `${Umbraco.Sys.ServerVariables.Plumber.viewsPath}overlays/html.overlay.html`,
                hideSubmitButton: true,
                title,
                content,
                submit: () => this.overlayService.close(),
                close: () => this.overlayService.close()
            };
            this.overlayService.open(overlayModel);
        };
        this.scaffold = (params) => {
            params.variant = this.safeVariant(params.variant);
            const result = this.request('GET', this.urls.scaffold + 'Get?' + this.generateQuery(params));
            this.nodeConfig = result.nodeConfig;
            this.configVariant = params.variant;
            return result;
        };
        this.generateQuery = (params) => Object.entries(params).map((e) => e[0] + '=' + e[1]).join('&');
        /*
         * Don't send * as a param - api endpoints default to *
         */
        this.safeVariant = (variant) => variant === '*' ? '' : variant;
        /* TASKS */
        this.getAllTasksForGroup = (query) => this.request('GET', this.urls.tasks + 'Group?' + this.generateQuery(query));
        this.getAllTasksForGroupForRange = (groupId, days) => this.request('GET', this.urls.tasks + `GetForGroupAndRange?groupId=${groupId}&days=${days}`);
        this.getAllTasksByGuid = (guid) => this.request('GET', `${this.urls.tasks}TasksByInstanceGuid?guid=${guid}`);
        /* INSTANCES */
        this.getAllInstances = (query) => this.request('GET', this.urls.instances + 'GetAll?' + this.generateQuery(query));
        this.getInstancesInitiatedByUser = (query) => this.request('GET', `${this.urls.instances}GetInitiatedBy?` + this.generateQuery(query));
        this.getInstancesAssignedToUser = (query) => this.request('GET', `${this.urls.instances}GetAssignedTo?` + this.generateQuery(query));
        this.getActiveInstances = (query) => this.request('GET', `${this.urls.instances}GetActive?` + this.generateQuery(query));
        this.getAllInstancesForRange = (days) => this.request('GET', `${this.urls.instances}GetRange?days=${days}`);
        this.getStatus = (ids) => this.request('GET', this.urls.instances + 'GetStatus?ids=' + ids);
        this.getDiff = (id) => this.request('GET', this.urls.instances + 'GetDiff?guid=' + id);
        /* workflow actions */
        this.initiateWorkflow = (model) => this.request('POST', this.urls.actions + 'Initiate', model);
        this.actionWorkflow = (model) => this.request('POST', this.urls.actions + model.action, model);
        /* SAVE PERMISSIONS */
        this.saveNodeConfig = (nodeId, permissions, variant, appliesTo) => {
            this.nodeConfig = null;
            this.configVariant = null;
            return this.request('POST', this.urls.config + 'SaveNodeConfig', {
                id: nodeId,
                permissions: permissions,
                variant: variant,
                type: appliesTo,
            });
        };
        this.saveDocTypeConfig = (syncModel) => this.request('POST', this.urls.config + 'SaveContentTypeConfig', syncModel);
        this.getNewNodeConfig = () => this.request('GET', this.urls.config + 'GetNewNodeConfig');
        this.getPathAndType = (nodeId) => this.request('GET', this.urls.config + 'GetPathAndType?id=' + nodeId);
        // pass the activity filter between the admin and history views 
        this.setActivityFilter = (filter) => {
            if (filter != null) {
                filterpicker_1.generateFilters(filter, false, this.dateHelper);
            }
            this.activityFilter = filter;
        };
        this.getActivityFilter = () => this.activityFilter;
    }
}
exports.WorkflowService = WorkflowService;
WorkflowService.serviceName = 'plmbrWorkflowResource';

},{"../../components/filterpicker/filterpicker":12,"./service-base":39}],42:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.OfflineModule = void 0;
const offline_controller_1 = require("./offline.controller");
const iframeloaded_directive_1 = require("./iframeloaded.directive");
exports.OfflineModule = angular
    .module('plumber.offline', [])
    .controller(offline_controller_1.OfflineController.controllerName, offline_controller_1.OfflineController)
    .directive(iframeloaded_directive_1.IframeLoaded.directiveName, () => new iframeloaded_directive_1.IframeLoaded())
    .name;

},{"./iframeloaded.directive":44,"./offline.controller":45}],43:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const _componentsModule_1 = require("../components/_componentsModule");
const _directivesModule_1 = require("../js/directives/_directivesModule");
const _servicesModule_1 = require("../js/services/_servicesModule");
const _offlineModule_1 = require("./_offlineModule");
const name = 'plumber';
const interceptorName = 'plumberOfflineInterceptor';
angular.module(name, [
    'umbraco.resources',
    'umbraco.services',
    'umbraco.packages',
    'umbraco.directives',
    'ngRoute',
    'ngSanitize',
    _servicesModule_1.ServicesModule,
    _directivesModule_1.DirectivesModule,
    _componentsModule_1.ComponentsModule,
    _offlineModule_1.OfflineModule
]).config($httpProvider => $httpProvider.interceptors.push(interceptorName));
function interceptor($q) {
    return {
        request: req => {
            // since we're outside Umbraco, routes need to be modified
            if (req.url.includes('LocalizedText') || req.url.includes('views/components')) {
                req.url = `/umbraco/${req.url}`;
            }
            return req || $q.when(req);
        }
    };
}
angular.module(name).factory(interceptorName, ['$q', interceptor]);

},{"../components/_componentsModule":1,"../js/directives/_directivesModule":23,"../js/services/_servicesModule":34,"./_offlineModule":42}],44:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.IframeLoaded = void 0;
class IframeLoaded {
    constructor() {
        this.restrict = 'A';
    }
    link(scope, element) {
        element.on('load', () => {
            var _a;
            const iframe = element.contentWindow || element.contentDocument;
            if (iframe && iframe.document.getElementById('umbracoPreviewBadge')) {
                iframe.document.getElementById('umbracoPreviewBadge').style.display = 'none';
            }
            const resultFrame = document.getElementById('resultFrame');
            if (!((_a = resultFrame === null || resultFrame === void 0 ? void 0 : resultFrame.contentWindow) === null || _a === void 0 ? void 0 : _a['refreshLayout'])) {
                scope.vm.frameLoaded = true;
                scope.$apply();
            }
        });
    }
}
exports.IframeLoaded = IframeLoaded;
IframeLoaded.directiveName = 'iframeLoaded';

},{}],45:[function(require,module,exports){
"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
exports.OfflineController = void 0;
const constants_1 = require("../js/constants");
const enums_1 = require("../js/models/enums");
class OfflineController {
    constructor($rootScope, $timeout, plmbrStateFactory, plmbrWorkflowResource, plmbrActionsService) {
        this.getState = () => {
            this.workflowResource.getPathAndType(this.nodeId)
                .then(pathAndType => {
                this.stateFactory.getOfflineState({ content: pathAndType })
                    .then(state => {
                    if (!state.currentTask.userGroup.offlineApproval) {
                        this.invalid = true;
                        return;
                    }
                    this.detail = () => this.actionsService.offlineDetail(state.currentTask, state);
                });
            });
        };
        /**
         * Extract cookie
         * @param {any} a = the cookie key
         */
        this.getCookie = (a) => {
            let d = [];
            let e = document.cookie.split(';');
            let regex = RegExp(`^\\s*${a}=\\s*(.*?)\\s*$`);
            for (let b = 0; b < e.length; b++) {
                const f = e[b].match(regex);
                if (f) {
                    d.push(f[1]);
                }
            }
            return d;
        };
        this.workflowResource = plmbrWorkflowResource;
        this.stateFactory = plmbrStateFactory;
        this.actionsService = plmbrActionsService;
        this.frameLoaded = false;
        // if the cookie exists, the request is invalid 
        this.invalid = this.getCookie('Workflow_Offline').length > 0;
        const segments = window.location.pathname.split('/');
        if (segments && segments.length === 7) {
            // domain/path/nodeid/culture/userid/taskid/guid
            const [, , nodeId, culture, , ,] = segments;
            this.culture = culture === '0' ? '*' : culture;
            this.nodeId = +nodeId;
            // v8 path can have aspx, v9 can not
            this.pageUrl = `/${this.nodeId}?culture=${this.culture === '*' ? '' : culture}`;
            if (!this.invalid) {
                this.getState();
            }
        }
        else {
            this.invalid = true;
        }
        this.showFooter = !this.invalid;
        $rootScope.$on(constants_1.constants.events.workflowActioned, (_, data) => {
            if (this.nodeId !== data.nodeId)
                return;
            if (data.status.includes(enums_1.WorkflowStatus.Approved)) {
                this.showFooter = false;
            }
            else {
                this.invalid = true;
            }
        });
        $rootScope.$on('appState.editors.open', (_, args) => args && args.editor.view.endsWith('workflow.detail.overlay.html')
            ? this.infiniteMode = true : {});
        $rootScope.$on('appState.editors.close', (_, args) => args && args.editor.view.endsWith('workflow.detail.overlay.html')
            ? $timeout(() => this.infiniteMode = false, 320) : {});
    }
}
exports.OfflineController = OfflineController;
OfflineController.controllerName = 'workflow.offline.controller';

},{"../js/constants":22,"../js/models/enums":28}]},{},[43,44,45,42]);

//# sourceMappingURL=plumber-offline.js.map
