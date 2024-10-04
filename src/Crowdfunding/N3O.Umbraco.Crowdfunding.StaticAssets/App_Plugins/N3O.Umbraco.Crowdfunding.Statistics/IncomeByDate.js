angular.module("umbraco")
    .controller("incomebydate", function ($scope, editorState, contentResource, assetsService) {

        //income by date
        class IncomeByDateGraph {
            constructor(selector, income_data) {
                this.income_data = income_data;
                this.margin = {top: 20, right: 0, bottom: 30, left: 80};
                this.width = 750 - this.margin.left - this.margin.right;
                this.height = 300 - this.margin.top - this.margin.bottom;

                this.svg = d3.select(selector)
                    .append("svg")
                    .attr("width", this.width + this.margin.left + this.margin.right)
                    .attr("height", this.height + this.margin.top + this.margin.bottom)
                    .append("g")
                    .attr("transform", `translate(${this.margin.left},${this.margin.top})`);


                this.initChart();
            }

            initChart() {
                this.x = d3.scaleUtc()
                    .domain(d3.extent(this.income_data, d => d.Date))
                    .range([0, this.width]);

                this.y = d3.scaleLinear()
                    .domain([0, d3.max(this.income_data, d => d.Total.Amount)])
                    .range([this.height, 0]);

                this.area = d3.area()
                    .x(d => this.x(d.Date))
                    .y0(this.height)
                    .y1(d => this.y(d.Total.Amount))
                    .curve(d3.curveCatmullRom);

                const line = d3.line()
                    .x(d => this.x(d.Date))
                    .y(d => this.y(d.Total.Amount))
                    .curve(d3.curveCatmullRom);

                // Define the gradient
                const gradient = this.svg.append("defs")
                    .append("linearGradient")
                    .attr("id", "areaGradient")
                    .attr("x1", "0%")
                    .attr("y1", "0%")
                    .attr("x2", "0%")
                    .attr("y2", "100%");

                gradient.append("stop")
                    .attr("offset", "0%")
                    .attr("stop-color", "#4F9B90")
                    .attr("stop-opacity", 1);

                gradient.append("stop")
                    .attr("offset", "100%")
                    .attr("stop-color", "#4F9B90")
                    .attr("stop-opacity", 0);

                this.svg.append("path")
                    .datum(this.income_data)
                    .attr("class", "line")
                    .attr("d", line)
                    .style("fill", "none")
                    .style("stroke", "#4F9B90")
                    .style("stroke-width", 2);

                this.svg.append("path")
                    .datum(this.income_data)
                    .attr("class", "area")
                    .attr("d", this.area)
                    .style("fill", "url(#areaGradient)");

                this.svg.append("g")
                    .attr("transform", `translate(0,${this.height})`)
                    .call(d3.axisBottom(this.x));

                this.svg.append("g")
                    .call(d3.axisLeft(this.y));

                this.addTooltip();
            }

            addTooltip() {
                const tooltip = d3.select("body").append("div")
                    .attr("class", "tooltip")
                    .style("opacity", 0);

                this.svg.selectAll("circle")
                    .data(this.income_data)
                    .enter().append("circle")
                    .attr("r", 5)
                    .attr("cx", d => this.x(d.Date))
                    .attr("cy", d => this.y(d.Total.Amount))
                    .style("fill", "#4F9B90")
                    .on("mouseover", function (event, d) {
                        tooltip.transition()
                            .duration(200)
                            .style("opacity", .9);
                        tooltip.html(`Date: ${d.Date.toDateString()}<br/>This period: £${d.Total.Amount}k<br/>`)
                            .style("left", (event.pageX + 5) + "px")
                            .style("top", (event.pageY - 28) + "px");
                    })
                    .on("mouseout", function (d) {
                        tooltip.transition()
                            .duration(500)
                            .style("opacity", 0);
                    });
            }
        }

        function renderIncomeByDateGraph(selector, incomeData) {
            const incomebydategraph = new IncomeByDateGraph(selector, incomeData);
        }

        const IncomeByDateData = [
            {Date: new Date(2023, 5, 21), Total: {Currency: "GBP", Amount: 90, Text: "£90"}},
            {Date: new Date(2023, 6, 22), Total: {Currency: "GBP", Amount: 50, Text: "£50"}},
            {Date: new Date(2023, 7, 23), Total: {Currency: "GBP", Amount: 70, Text: "£70"}},
            {Date: new Date(2023, 8, 24), Total: {Currency: "GBP", Amount: 100, text: "£100"}},
            {Date: new Date(2023, 9, 25), Total: {Currency: "GBP", Amount: 90, Text: "£90"}},
            {Date: new Date(2023, 10, 26), Total: {Currency: "GBP", Amount: 120, Text: "£120"}},
            {Date: new Date(2023, 11, 27), Total: {Currency: "GBP", Amount: 150, Text: "£150"}},
            {Date: new Date(2023, 12, 28), Total: {Currency: "GBP", Amount: 170, Text: "£170"}},
            {Date: new Date(2024, 1, 29), Total: {Currency: "GBP", Amount: 150, Text: "£150"}}
        ];

        renderIncomeByDateGraph(".income-by-date-graph", IncomeByDateData);


        assetsService.loadCss("~/App_Plugins/N3O.Umbraco.Crowdfunding.Statistics/N3O.Umbraco.Crowdfunding.Statistics.css");
    });