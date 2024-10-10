angular.module("umbraco")
    .controller("topallocations", function ($scope, editorState, contentResource, assetsService) {

        //top allocations
        class RaceBarChart {

            constructor(selector) {
                this.data = [
                    {Summary: "Gaza Fund", Total: {Amount: 30, Currency: "GBP", Text: "£30"}},
                    {Summary: "Dig-a-Well", Total: {Amount: 20, Currency: "GBP", Text: "£20"}},
                    {Summary: "Community Well", Total: {Amount: 20, Currency: "GBP", Text: "£20"}},
                    {Summary: "Where Most Needed", Total: {Amount: 15, Currency: "GBP", Text: "£15"}},
                    {Summary: "Winter Walk for Gaza", Total: {Amount: 10, Currency: "GBP", Text: "£10"}}
                ];

                this.width = 380;
                this.height = 270;
                this.margin = {top: 20, right: 0, bottom: 40, left: 20};

                this.svg = d3.select(selector)
                    .append("svg")
                    .attr("width", this.width)
                    .attr("height", this.height);

                this.x = d3.scaleLinear()
                    .domain([0, d3.max(this.data, d => d.Total.Amount)])
                    .nice()
                    .range([this.margin.left, this.width - this.margin.right]);

                this.y = d3.scaleBand()
                    .domain(this.data.map(d => d.Summary))
                    .range([this.margin.top, this.height - this.margin.bottom])
                    .padding(0.1);

                this.initChart();
            }

            initChart() {
                this.svg.selectAll(".bar")
                    .data(this.data)
                    .enter()
                    .append("rect")
                    .attr("class", "bar")
                    .attr("x", this.margin.left)
                    .attr("y", d => this.y(d.Summary))
                    .attr("width", 0) // Start with width 0 for animation
                    .attr("height", this.y.bandwidth())
                    .attr("fill", "#D8E8E6")
                    .transition()
                    .duration(1000)
                    .attr("width", d => this.x(d.Total.Amount) - this.margin.left);

                // Hide the x and y axes
                this.svg.append("g")
                    .attr("transform", `translate(0,${this.height - this.margin.bottom})`)
                    .call(d3.axisBottom(this.x))
                    .style("display", "none");

                this.svg.append("g")
                    .attr("transform", `translate(${this.margin.left},0)`)
                    .call(d3.axisLeft(this.y))
                    .style("display", "none");

                // Add labels to the bars
                this.svg.selectAll(".label")
                    .data(this.data)
                    .enter()
                    .append("text")
                    .attr("class", "label")
                    .attr("x", this.margin.left + 5) // Position inside the bar
                    .attr("y", d => this.y(d.Summary) + this.y.bandwidth() / 2)
                    .attr("dy", ".35em")
                    .attr("fill", "#000") // Ensure text is visible
                    .text(d => d.Summary);

                // Add static counts to the right
                this.svg.selectAll(".count")
                    .data(this.data)
                    .enter()
                    .append("text")
                    .attr("class", "count")
                    .attr("x", this.width - this.margin.right - 5) // Position at the right end
                    .attr("y", d => this.y(d.Summary) + this.y.bandwidth() / 2)
                    .attr("dy", ".35em")
                    .attr("text-anchor", "end")
                    .attr("fill", "#000") // Ensure text is visible
                    .text(d => d.Total.Amount);
            }
        }

        // Function to render the race bar chart
        function renderRaceBarChart(selector) {
            const raceBarChart = new RaceBarChart(selector);
        }

        // Call the function to render the chart
        renderRaceBarChart(".top-allocations-graph");


        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });