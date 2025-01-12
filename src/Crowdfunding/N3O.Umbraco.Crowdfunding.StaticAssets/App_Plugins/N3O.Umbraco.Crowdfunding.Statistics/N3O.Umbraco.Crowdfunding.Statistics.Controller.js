angular.module("umbraco")
    .controller("N3O.Umbraco.Crowdfunding.Statistics", function ($scope, editorState, contentResource, assetsService, statisticsHelper) {
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
                let res = await fetch(`/umbraco/backoffice/api/crowdfundingBackOffice/environments`);

                let environments = await res.json();

                const dropdown = document.getElementById('stats-source');

                environments.forEach(environment => {
                    const newOption = document.createElement('option');
                    newOption.value = environment.crowdfundingEnvironment;
                    newOption.textContent = environment.name;
                    newOption.dataset.domain = environment.domain;
                    newOption.dataset.apiKey = environment.apiKey;
                    
                    dropdown.appendChild(newOption);

                    if(environment.default) {
                        dropdown.value = environment.crowdfundingEnvironment;
                    }
                });

                if(environments.length === 1 && environments.some(item => item.default)) {
                    document.querySelector('.stats-source-container').style.display = 'none';
                }

                dropdown.addEventListener('change', async function (event) {
                    await initializeData();
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

                let source = document.getElementById('stats-source');
                let environment = source.options[source.selectedIndex];

                let res = await fetch(`${environment.dataset.domain}/umbraco/api/crowdfundingStatistics/dashboard`, {
                    method: "POST",
                    headers: {
                        "Accept": "application/json",
                        "Content-Type": "application/json",
                        "Crowdfunding-API-Key": environment.dataset.apiKey
                    },
                    body: JSON.stringify(req)
                });

                if(res.status !== 200) {
                    document.querySelector('.n3o-crowdfunding-stats').innerHTML = `There was an error fetching data, please refresh the page and if the error persists, please contact support`;

                    throw Error(`Error fetching data.`);
                }

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