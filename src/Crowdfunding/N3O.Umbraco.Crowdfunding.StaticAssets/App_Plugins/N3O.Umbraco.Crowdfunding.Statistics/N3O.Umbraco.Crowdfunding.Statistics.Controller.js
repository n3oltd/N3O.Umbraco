angular.module("umbraco")
    .controller("N3O.Umbraco.Crowdfunding.Statistics", function ($scope, editorState, contentResource, assetsService) {

        class ProgressBarC {
            constructor(selector, data) {
                this.selector = selector;
                this.data = data;
                this.percentage = (this.data.ContributionsTotal / this.data.GoalsTotal) * 100;
                this.updateTitle();
                this.createProgressBarC();
            }

            updateTitle() {
                // Find the parent row of the progress bar
                const parentRow = document.querySelector(this.selector).closest('.campaigns-row');
                if (parentRow) {
                    // Update the title span within the parent row
                    const titleSpan = parentRow.querySelector('span:first-child');
                    if (titleSpan) {
                        titleSpan.textContent = this.data.Name;
                    }
                }
            }

            createProgressBarC() {
                const svg = d3.select(this.selector)
                    .append("svg")
                    .attr("width", 100)
                    .attr("height", 50);

                svg.append("text")
                    .attr("x", 40)
                    .attr("y", 20)
                    .attr("text-anchor", "middle")
                    .attr("fill", "#000")
                    .style("font-size", "1rem")
                    .text(`${this.data.ContributionsTotal}/${this.data.GoalsTotal}`);

                svg.append("rect")
                    .attr("width", 80)
                    .attr("height", 7)
                    .attr("fill", "#d4ebe8")
                    .attr("rx", 5)
                    .attr("ry", 5)
                    .attr("y", 30);

                svg.append("rect")
                    .attr("width", this.percentage)
                    .attr("height", 7)
                    .attr("fill", "#87C7BE")
                    .attr("rx", 5)
                    .attr("ry", 5)
                    .attr("y", 30);
            }
        }

        // Function to render all progress bars
        function renderAllProgressBars() {
            const progressBarData = [
                { Name: "Gaza Crisis", GoalsTotal: 55, ContributionsTotal: 25 },
                { Name: "Ramadan Appeal", GoalsTotal: 100, ContributionsTotal: 50 },
                { Name: "Winter Walk for Gaza", GoalsTotal: 75, ContributionsTotal: 60 },
                { Name: "Campaign 4", GoalsTotal: 200, ContributionsTotal: 150 },
                { Name: "Campaign 5", GoalsTotal: 300, ContributionsTotal: 200 },
                { Name: "Campaign 6", GoalsTotal: 400, ContributionsTotal: 300 },
                { Name: "Campaign 7", GoalsTotal: 500, ContributionsTotal: 400 },
                { Name: "Campaign 8", GoalsTotal: 600, ContributionsTotal: 500 },
                { Name: "Campaign 9", GoalsTotal: 700, ContributionsTotal: 600 },
                { Name: "Campaign 10", GoalsTotal: 800, ContributionsTotal: 700 }
            ];

            progressBarData.forEach((data, index) => {
                const selector = `.c-progress-bar-${index + 1}`;
                new ProgressBarC(selector, data);
            });
        }

        // Call the function to render all progress bars
        renderAllProgressBars();

        class ProgressBarF {
            constructor(selector, data) {
                this.selector = selector;
                this.data = data;
                this.percentage = (this.data.ContributionsTotal / this.data.GoalsTotal) * 100;
                this.updateTitle();
                this.createProgressBarF();
            }

            updateTitle() {
                // Find the parent row of the progress bar
                const parentRow = document.querySelector(this.selector).closest('.fundraisers-row');
                if (parentRow) {
                    // Update the title span within the parent row
                    const titleSpan = parentRow.querySelector('span:first-child');
                    if (titleSpan) {
                        titleSpan.textContent = this.data.Name;
                    }
                }
            }

            createProgressBarF() {
                const svg = d3.select(this.selector)
                    .append("svg")
                    .attr("width", 100)
                    .attr("height", 50);

                svg.append("text")
                    .attr("x", 40)
                    .attr("y", 20)
                    .attr("text-anchor", "middle")
                    .attr("fill", "#000")
                    .style("font-size", "1rem")
                    .text(`${this.data.ContributionsTotal}/${this.data.GoalsTotal}`);

                svg.append("rect")
                    .attr("width", 80)
                    .attr("height", 7)
                    .attr("fill", "#c5d0e3")
                    .attr("rx", 5)
                    .attr("ry", 5)
                    .attr("y", 30);

                svg.append("rect")
                    .attr("width", this.percentage)
                    .attr("height", 7)
                    .attr("fill", "#6A87B8")
                    .attr("rx", 5)
                    .attr("ry", 5)
                    .attr("y", 30);
            }
        }

        // Function to render all fundraiser progress bars
        function renderAllProgressBarsF() {
            const progressBarDataF = [
                { Name: "Fundraiser 1", GoalsTotal: 55, ContributionsTotal: 25 },
                { Name: "Fundraiser 2", GoalsTotal: 100, ContributionsTotal: 45 },
                { Name: "Fundraiser 3", GoalsTotal: 75, ContributionsTotal: 30 },
                { Name: "Fundraiser 4", GoalsTotal: 200, ContributionsTotal: 120 },
                { Name: "Fundraiser 5", GoalsTotal: 300, ContributionsTotal: 180 },
                { Name: "Fundraiser 6", GoalsTotal: 400, ContributionsTotal: 250 },
                { Name: "Fundraiser 7", GoalsTotal: 500, ContributionsTotal: 320 },
                { Name: "Fundraiser 8", GoalsTotal: 600, ContributionsTotal: 400 },
                { Name: "Fundraiser 9", GoalsTotal: 700, ContributionsTotal: 500 },
                { Name: "Fundraiser 10", GoalsTotal: 800, ContributionsTotal: 600 }
            ];

            progressBarDataF.forEach((data, index) => {
                const selector = `.f-progress-bar-${index + 1}`;
                new ProgressBarF(selector, data);
            });
        }

        // Call the function to render all fundraiser progress bars
        renderAllProgressBarsF();

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });
