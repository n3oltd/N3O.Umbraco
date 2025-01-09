/**
 @param {array} items (<code>binding</code>): The items for the table.
 @param {array} itemProperties (<code>binding</code>): The properties for the items to use in table.
 @param {boolean} allowSelectAll (<code>binding</code>): Specify whether to allow select all.
 @param {function} onSelect (<code>expression</code>): Callback function when the row is selected.
 @param {function} onClick (<code>expression</code>): Callback function when the "Name" column link is clicked.
 @param {function} onSelectAll (<code>expression</code>): Callback function when selecting all items.
 @param {function} onSelectedAll (<code>expression</code>): Callback function when all items are selected.
 @param {function} onSortingDirection (<code>expression</code>): Callback function when sorting direction is changed.
 @param {function} onSort (<code>expression</code>): Callback function when sorting items.
 @param {function} onFilterUpdated (<code>expression</code>): Callback function when filter updated.
 @param {array} actions (<code>binding</code>): The properties for the Actions to be used in table.
 **/

(function() {
    'use strict';

    function TableController($scope, iconHelper, assetsService, listViewHelper) {
        var vm = this;
        vm.selection = [];
        vm.showFilters = false;
        vm.showSearchBar = false;
        vm.dropdownOpen = false;
        $scope.$on('clearSelection', () => {
            vm.clearSelected();
        })

        vm.onActionsClick = function(itemId) {
            const popover = document.getElementById(`actions-popover-${itemId}`);
            popover.open = !popover.open;
        };
        
        vm.$onInit = function() {
            vm.filters = [...vm.getFilters()]
            if (vm.items) {
                vm.rows = [...vm.items]
            }
            
            vm.globalActions = vm.actions?.filter(x => x.global === true || x.global === undefined);
        };

        vm.$onChanges = function (changesObj) {
            if (changesObj.items) {
                vm.filters = [...vm.getFilters(true)]
                vm?.onFilterChange(true);
            }
        };

        vm.clickItem = function(item, column, $event) {
            if (vm.onClick && !($event.metaKey || $event.ctrlKey)) {
                $event.preventDefault();
                column.onClick({item: item});
            }

            $event.stopPropagation();
        };

        vm.selectItem = function(selectedItem, $index, $event) {
            listViewHelper.selectHandler(selectedItem, $index, vm.items, vm.selection, $event);
            
            $event.stopPropagation();
        };

        vm.selectAll = function($event) {
            listViewHelper.selectAllItemsToggle(vm.items, vm.selection);
        };

        vm.getIcon = function(entry) {
            return iconHelper.convertFromLegacyIcon(entry.icon);
        };

        vm.isSelectedAll = function() {
            return listViewHelper.isSelectedAll(vm.items, vm.selection);
        };

        vm.clearSelected = function() {
            listViewHelper.clearSelection(vm.items, null, vm.selection);
        };

        vm.isSortDirection = {
            column: '',
            descending: false
        };


        vm.sort = function(column) {

            var sort = vm.isSortDirection;

            if (sort.column == column) {
                sort.descending = !sort.descending;
            } else {
                sort.column = column;
                sort.descending = false;
            }
        };

        vm.loadMore = function(continuationToken) {
            if (vm.onLoadMore) {
                vm.onLoadMore({continuationToken: continuationToken});
            }
        };
        vm.onFilterChange = function (fromChanges) {

            if (fromChanges && vm.onFiltersChange) {
                vm.rows = [...vm.items]
                return;
            }

            let filtersToApply = vm.filters.filter(x => x.value != x.options[0]);

            if (vm.onFiltersChange) {
                vm.onFiltersChange(filtersToApply);
                vm.rows = [...vm.items]
                return;
            }

            if (filtersToApply.length > 0) {
                let filteredResult = [...vm.items];
                filtersToApply.forEach(x => {
                    filteredResult = [...filteredResult.filter(y => y[x.alias] == x.value)]
                })
                vm.rows = [...filteredResult]
            } else if (vm.rows?.length != vm.items?.length && filtersToApply.length == 0) {
                vm.rows = [...vm.items]
            }
        }

        vm.getFilters = function(preserveValue = false) {
            let columnFilterDetail = [];

            if (vm.customFilters) {
                return vm.customFilters; 
            }
            
            if (!vm?.itemProperties?.filter) {
                return columnFilterDetail;
            }
            
            let columnFilters = vm?.itemProperties?.filter(x => x?.canFilter);

            columnFilters?.forEach(element => {
                let filterValue = (preserveValue && vm.filters?.find(x => x.alias == element.alias && x.name == element.header)?.value)
                    ? vm.filters?.find(x => x.alias == element.alias && x.name == element.header)?.value
                    : `filter by ${element.header}`;

                const filterOptions = vm.items.map(x => {
                    return x[element.alias]
                }).filter((v, i, a) => v && a.indexOf(v) === i);

                if (filterOptions.length) {
                    columnFilterDetail.push(
                        {
                            name: element.header,
                            alias: element.alias,
                            value: filterValue,
                            options: [`filter by ${element.header}`,
                                ...filterOptions
                            ]
                        }
                    )    
                }
            });

            return columnFilterDetail;
        }

        vm.bulkAction = function(action) {
            let selectedIds = vm.selection.reduce((acc, item) => {
                return [...acc, item.id]
            }, [])
            action?.onBulkClick(vm.items.filter(x => selectedIds.includes(x.id)))
        }

        vm.onShowFiltersChange = function() {
			vm.showFilters = !vm.showFilters;
            
            if (vm.showFilters && vm.showSearchBar) {
                vm.showSearchBar = !vm.showSearchBar
            }
        };

        vm.onShowSearchBarChange = function() {
			vm.showSearchBar = !vm.showSearchBar;

            if (vm.showFilters && vm.showSearchBar) {
                vm.showFilters = !vm.showFilters
            }
        }

        vm.handleOnInstruct = (all) => {
            vm?.onInstruct(vm.selection.map(s => s.id), all);
        }

        vm.handleOnSearchChange = () => {
            if (!vm.onSeachTermChange) {
                return
            }

            if (vm.timerId) {
                clearTimeout(vm.timerId);
            }

            vm.timerId = setTimeout(() => { vm.onSeachTermChange(vm.searchTerm) }, 500);
        }

        assetsService.loadCss("~/umbraco/lib/n3o-table/n3o-table.css");
    }

    angular
        .module('umbraco')
        .component('n3oTable', {
            templateUrl: '/umbraco/lib/n3o-table/n3o-table.html',
            controller: TableController,
            controllerAs: 'vm',
            bindings: {
                items: '<',
                itemProperties: '<',
                allowSelect: '<',
                allowSelectAll: '<',
                allowLoadMore: '<',
                onLoadMore: '&',
                onSelect: '&',
                onClick: '&',
                onSelectAll: '&',
                onSelectedAll: '&',
                bulkActions: '<',
                onClearAll: '&',
                onFilterUpdated: '<',
                actions: '<',
                enableSearch: '<',
                isLoadingMore: '<',
                onFiltersChange: '<',
                customFilters: '<',
                instructAll: '<',
                onInstruct: '<',
                onSeachTermChange: '<',
                note: '<',
                isLoading: '<'
            }
        });
})();