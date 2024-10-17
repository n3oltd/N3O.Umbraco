angular.module("umbraco")
    .controller("fundraisersbycampaign", function ($scope, editorState, contentResource, assetsService) {

        class FundraisersByCampaignGraph {
            constructor(selector, data) {
                this.data = data;
                this.width = 380;
                this.height = 270;
                this.margin = {top: 30, right: 10, bottom: 20, left: 40};
                this.radius = Math.min(this.width, this.height) / 2 - this.margin.top;

                this.svg = d3.select(selector)
                    .append("svg")
                    .attr("width", this.width)
                    .attr("height", this.height)
                    .append("g")
                    .attr("transform", `translate(${this.width / 2 + 80}, ${this.height / 2})`);

                this.color = d3.scaleOrdinal()
                    .domain(this.data.map(d => d.CampaignName))
                    .range(["#4F9B90", "#2152A3", "#507cc5", "#F5C1BC", "#FDD09FB2", "#FBE38EB2"]);

                this.pie = d3.pie()
                    .value(d => d.Count)
                    .sort(null);

                this.arc = d3.arc()
                    .innerRadius(this.radius * 0.5)
                    .outerRadius(this.radius);

                this.initChart();
            }

            initChart() {
                const arcs = this.svg.selectAll(".arc")
                    .data(this.pie(this.data))
                    .enter()
                    .append("g")
                    .attr("class", "arc");

                arcs.append("path")
                    .attr("d", this.arc)
                    .attr("fill", d => this.color(d.data.CampaignName));

                const legend = this.svg.append("g")
                    .attr("transform", `translate(${-this.width / 2 - 70}, ${-this.height / 2 + 20})`);

                legend.selectAll("rect")
                    .data(this.data)
                    .enter()
                    .append("rect")
                    .attr("x", 0)
                    .attr("y", (d, i) => i * 20)
                    .attr("width", 18)
                    .attr("height", 18)
                    .attr("fill", d => this.color(d.CampaignName));

                legend.selectAll("text")
                    .data(this.data)
                    .enter()
                    .append("text")
                    .attr("x", 24)
                    .attr("y", (d, i) => i * 20 + 9)
                    .attr("dy", "0.35em")
                    .text(d => d.CampaignName);
            }
        }

        // Function to render the donut chart
        function renderFundraisersByCampaignGraph(selector, data) {
            new FundraisersByCampaignGraph(selector, data);
        }

        // Example usage to render the graph
        const donutGraphData = [
            {CampaignName: "Gaza Crisis", Count: 30},
            {CampaignName: "Ramadan Appeal", Count: 20},
            {CampaignName: "Winter Walk for Gaza", Count: 25},
            {CampaignName: "Campaign 1", Count: 15},
            {CampaignName: "Campaign 2", Count: 10},
            {CampaignName: "Campaign 3", Count: 20}
        ];

        // Call the function to render the graph
        renderFundraisersByCampaignGraph(".fundraisers-by-campaign-graph", donutGraphData);

        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });
