angular.module("umbraco")
    .controller("N3O.Umbraco.Crowdfunding.Statistics", function ($scope, $cookies, editorState, contentResource, assetsService, statisticsHelper, authResource) {
        (async () => {
            let d3Script = document.createElement('script');
            d3Script.src = "https://d3js.org/d3.v7.min.js";
            d3Script.setAttribute('charset', 'utf-8');
            
            d3Script.onload = async function () {
                await initializeEnvironments();
                
                initializeDates();

                await initializeData();
            };
            
            document.head.appendChild(d3Script);
            
            async function initializeEnvironments() {
                let res = await fetch(`/umbraco/backoffice/api/CrowdfundingBackOffice/Environments`);

                let options = await res.json();

                const dropdown = document.getElementById('stats-source');

                options.forEach(option => {
                    const newOption = document.createElement('option');
                    newOption.value = option.statisticsEnvironment;
                    newOption.textContent = option.name;
                    newOption.dataset.domain = option.domain;
                    newOption.dataset.apiKey = option.apiKey;
                    
                    dropdown.appendChild(newOption);
                    
                    if(option.default) {
                        dropdown.value = option.statisticsEnvironment; 
                    }
                });
            }

            async function initializeData() {
                let data = await fetchData();

                document.getElementById('total-amount').textContent = data.contributions.total.text;
                document.getElementById('average-donation').textContent = data.contributions.average.text;
                document.getElementById('donation-count').textContent = data.contributions.count;

                document.getElementById("active-pages-count").textContent = data.fundraisers.activeCount;
                document.getElementById("completed-pages-count").textContent = data.fundraisers.completedCount;
                document.getElementById("new-pages-count").textContent = data.fundraisers.newCount;

                document.getElementById("total-fundraiser-percentage").textContent = `${data.fundraisers.averagePercentageComplete}%`;
                document.getElementById("total-fundraiser-count").textContent = data.fundraisers.count;
                document.getElementById("total-campaign-percentage").textContent = `${data.campaigns.averagePercentageComplete}%`;
                document.getElementById("total-campaign-count").textContent = data.campaigns.count;

                document.getElementById('regular-donations-count').textContent = data.contributions.regularDonationsCount;
                document.getElementById('single-donations-count').textContent = data.contributions.singleDonationsCount;
                document.getElementById('supporters-count').textContent = data.contributions.supportersCount;

                statisticsHelper.RenderRaceBarChart(".top-allocations-graph", data.allocations.topItems);
                statisticsHelper.RenderIncomeByDateGraph(".income-by-date-graph", data.contributions.daily);
                statisticsHelper.RenderFundraisersByCampaignGraph(".fundraisers-by-campaign-graph", data.fundraisers.byCampaign);

                data.campaigns.topItems.forEach((data, index) => {
                    const className = `c-progress-bar-${index + 1}`;
                    statisticsHelper.RenderProgressBar("Campaign", className, data);
                });

                data.fundraisers.topItems.forEach((data, index) => {
                    const className = `f-progress-bar-${index + 1}`;
                    statisticsHelper.RenderProgressBar("Fundraiser", className, data);
                });
            }
            
            async function fetchData() {
                let req = {};
                req.period = {};
                req.period.from = document.getElementById('start-date').value;
                req.period.to = document.getElementById('end-date').value;

                let environment = document.getElementById('stats-source').value;
                
                let res = await fetch(environment.dataset.domain, {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(req)
                });
                return res.json();
            }

            $scope.showContainer = function (containerId) {
                document.querySelectorAll('.container').forEach(container => {
                    container.classList.remove('active');
                });

                document.getElementById(containerId).classList.add('active');

                document.querySelectorAll('.tab').forEach(tab => {
                    tab.classList.remove('active');
                });

                document.querySelector(`.tab[ng-click="showContainer('${containerId}')"]`).classList.add('active');
            }
            
            function initializeDates() {
                const currentDate = new Date();
                const oneMonthAgo = new Date();
                oneMonthAgo.setMonth(currentDate.getMonth() - 1);
                
                document.getElementById('start-date').value = formatDate(oneMonthAgo);
                document.getElementById('end-date').value = formatDate(currentDate);

                document.getElementById('end-date').addEventListener('change', async function (event) {
                    clearCharts();

                    await initializeData();
                });

                document.getElementById('start-date').addEventListener('change', async function (event) {
                    clearCharts();

                    await initializeData();
                });
            }
            
            function clearCharts() {
                document.querySelectorAll('.crowdfunder-row').forEach((row, i) => {
                    row.remove();
                });

                document.querySelector(`.top-allocations-graph`).innerHTML = '';
                document.querySelector(`.income-by-date-graph`).innerHTML = '';
                document.querySelector(`.fundraisers-by-campaign-graph`).innerHTML = '';
            }

            function formatDate(date) {
                const year = date.getFullYear();
                const month = String(date.getMonth() + 1).padStart(2, '0');
                const day = String(date.getDate()).padStart(2, '0');
                return `${year}-${month}-${day}`;
            }

            document.querySelectorAll('.toggle-btn').forEach(button => {
                button.addEventListener('click', () => {
                    const content = button.nextElementSibling;
                    content.style.display = content.style.display === 'block' ? 'none' : 'block';

                    button.classList.toggle('active');
                });
            });
        })();

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });