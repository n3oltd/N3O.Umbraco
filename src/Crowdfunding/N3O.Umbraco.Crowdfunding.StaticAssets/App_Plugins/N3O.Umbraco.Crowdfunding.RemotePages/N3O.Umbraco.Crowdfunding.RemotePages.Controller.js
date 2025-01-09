angular.module("umbraco")
    .controller("N3O.Umbraco.Crowdfunding.RemotePages", function ($scope, assetsService) {
        assetsService.loadCss("~/umbraco/lib/n3o-table/n3o-table.css");

        (async () => {
            $scope.actions = [
                {
                    name: 'View Page',
                    onClick: (item) => window.open(item.url, '_blank').focus()
                },
                {
                    name: 'Edit Page',
                    onClick: async (item) => window.open(item.editUrl, '_blank').focus()
                }
            ]

            await initializePages();
        })();

        async function initializePages(currentPage) {
            let environment = await getProductionEnvironment();

            $scope.isLoading = true;

            let req = {
                type: 'fundraiser',
                pageSize: 5,
                currentPage: currentPage
            };

            await fetch(`${environment.domain}/umbraco/api/CrowdfundingStatistics/Pages`, {
                method: "POST",
                headers: {
                    "Accept": "application/json",
                    "Content-Type": "application/json",
                    "Crowdfunding-API-Key": environment.apiKey
                },
                body: JSON.stringify(req)
            })
                .then(res => res.json())
                .then(res => updatePages(res));
        }

        $scope.loadNextPage = async function() {
            $scope.loadingMore = true;
            await initializePages($scope.continuationToken);
            $scope.loadingMore = false;
        }

        async function updatePages(data) {
            if (!data || data.entries.length === 0) {
                $scope.items = [];
                $scope.isLoading = false;

                return;
            }

            if ($scope.items == null) {
                $scope.items = populateItems(data.entries);
                $scope.options = populateOptions();
                $scope.continuationToken = data.currentPage;
            } else {
                $scope.items = $scope.items.concat(populateItems(data.entries));
                $scope.continuationToken = data.currentPage;
            }

            $scope.isLoading = false;
            $scope.canLoadMore = data.hasMoreEntries;

            $scope.$digest();
        }

        function populateItems(pages) {
            let items = [];

            for (let i = 0; i < pages.length; i++) {
                let data = {
                    "id": pages[i].name,
                    "reference": pages[i].name,
                    "ownerName": pages[i].ownerName,
                    "status": pages[i].status,
                    "url": getPreviewUrl(pages[i].url),
                    "editUrl": pages[i].url
                };
                items.push(data);
            }

            return items;
        }

        function populateOptions() {
            let options = {includeProperties: []};

            options.includeProperties.unshift({alias: "reference", header: "Name", canFilter: false});
            options.includeProperties.push({alias: "ownerName", header: "Owner Name", canFilter: false});
            options.includeProperties.push({alias: "status", header: "Status", canFilter: false});

            return options;
        }

        async function getProductionEnvironment() {
            let res = await fetch(`/umbraco/backoffice/api/CrowdfundingBackOffice/Environments`);

            let environments = await res.json();

            let productionEnvironment = environments.filter((env) => env.crowdfundingEnvironment === 'production');

            if(!productionEnvironment || productionEnvironment.length > 1) {
                throw new Error('Error');
            }

            return productionEnvironment[0];
        }

        function getPreviewUrl(url) {
            let newUrl = new URL(url);
            newUrl.searchParams.append("preview", true);
            
            return newUrl;
        }

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });