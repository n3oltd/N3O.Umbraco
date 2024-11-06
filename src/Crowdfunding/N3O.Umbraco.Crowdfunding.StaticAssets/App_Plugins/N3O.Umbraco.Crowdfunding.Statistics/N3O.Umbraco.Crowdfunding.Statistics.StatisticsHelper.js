class StatisticsHelper {
    RenderRaceBarChart(selector, data) {
        const width = 380;
        const height = 270;
        const margin = {top: 20, right: 0, bottom: 40, left: 20};

        const svg = d3.select(selector)
            .append("svg")
            .attr("width", width)
            .attr("height", height);

        const x = d3.scaleLinear()
            .domain([0, d3.max(data, d => d.total.amount)])
            .nice()
            .range([margin.left, width - margin.right]);

        const y = d3.scaleBand()
            .domain(data.map(d => d.summary))
            .range([margin.top, height - margin.bottom])
            .padding(0.1);

        svg.selectAll(".bar")
            .data(data)
            .enter()
            .append("rect")
            .attr("class", "bar")
            .attr("x", margin.left)
            .attr("y", d => y(d.summary))
            .attr("width", 0) // Start with width 0 for animation
            .attr("height", y.bandwidth())
            .attr("fill", "#D8E8E6")
            .transition()
            .duration(1000)
            .attr("width", d => x(d.total.amount) - margin.left);

        // Hide the x and y axes
        svg.append("g")
            .attr("transform", `translate(0,${height - margin.bottom})`)
            .call(d3.axisBottom(x))
            .style("display", "none");

        svg.append("g")
            .attr("transform", `translate(${margin.left},0)`)
            .call(d3.axisLeft(y))
            .style("display", "none");

        // Add labels to the bars
        svg.selectAll(".label")
            .data(data)
            .enter()
            .append("text")
            .attr("class", "label")
            .attr("x", margin.left + 5) // Position inside the bar
            .attr("y", d => y(d.summary) + y.bandwidth() / 2)
            .attr("dy", ".35em")
            .attr("fill", "#000") // Ensure text is visible
            .text(d => d.summary);

        // Add static counts to the right
        svg.selectAll(".count")
            .data(data)
            .enter()
            .append("text")
            .attr("class", "count")
            .attr("x", width - margin.right - 5) // Position at the right end
            .attr("y", d => y(d.summary) + y.bandwidth() / 2)
            .attr("dy", ".35em")
            .attr("text-anchor", "end")
            .attr("fill", "#000") // Ensure text is visible
            .text(d => d.total.amount);
    }

    RenderProgressBar(containerClassName, rowClassName, data) {
        const htmlContent = `
            <span></span>
                <div class="progress-bars">
                    <div class="${rowClassName}"></div>
                </div>
                <a href="${data.url}">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 16 16" fill="none">
                    <path d="M12.4001 9.59998H11.6001C11.494 9.59998 11.3923 9.64212 11.3173 9.71713C11.2422 9.79215 11.2001 9.89389 11.2001 9.99998V12.8H3.2001V4.79998H6.8001C6.90618 4.79998 7.00793 4.75783 7.08294 4.68282C7.15795 4.6078 7.2001 4.50606 7.2001 4.39998V3.59998C7.2001 3.49389 7.15795 3.39215 7.08294 3.31713C7.00793 3.24212 6.90618 3.19998 6.8001 3.19998H2.8001C2.48184 3.19998 2.17661 3.3264 1.95157 3.55145C1.72653 3.77649 1.6001 4.08172 1.6001 4.39998L1.6001 13.2C1.6001 13.5182 1.72653 13.8235 1.95157 14.0485C2.17661 14.2735 2.48184 14.4 2.8001 14.4H11.6001C11.9184 14.4 12.2236 14.2735 12.4486 14.0485C12.6737 13.8235 12.8001 13.5182 12.8001 13.2V9.99998C12.8001 9.89389 12.758 9.79215 12.6829 9.71713C12.6079 9.64212 12.5062 9.59998 12.4001 9.59998ZM13.8001 1.59998H10.6001C10.0658 1.59998 9.79885 2.24773 10.1751 2.62498L11.0683 3.51823L4.9751 9.60922C4.91916 9.66497 4.87478 9.7312 4.8445 9.80413C4.81421 9.87706 4.79862 9.95526 4.79862 10.0342C4.79862 10.1132 4.81421 10.1914 4.8445 10.2643C4.87478 10.3372 4.91916 10.4035 4.9751 10.4592L5.54185 11.025C5.59759 11.0809 5.66383 11.1253 5.73676 11.1556C5.80969 11.1859 5.88788 11.2014 5.96685 11.2014C6.04582 11.2014 6.12401 11.1859 6.19694 11.1556C6.26987 11.1253 6.33611 11.0809 6.39185 11.025L12.4821 4.93298L13.3751 5.82498C13.7501 6.19998 14.4001 5.93748 14.4001 5.39998V2.19998C14.4001 2.04085 14.3369 1.88823 14.2244 1.77571C14.1118 1.66319 13.9592 1.59998 13.8001 1.59998V1.59998Z" fill="#1B264F"/>
                </svg>
                </a>`;
        
        let newDiv = document.createElement('div');
        newDiv.classList.add('crowdfunder-row');
        newDiv.innerHTML = htmlContent;

        const container = document.querySelector(`.${containerClassName}-totals`);

        container.parentNode.insertBefore(newDiv, container);

        const parentRow = document.querySelector(`.${rowClassName}`).closest('.crowdfunder-row');

        const percentage = (data.contributionsTotal.amount / data.goalsTotal.amount) * 100;

        if (parentRow) {
            const titleSpan = parentRow.querySelector('span:first-child');

            if (titleSpan) {
                titleSpan.textContent = data.name;
            }

            const svg = d3.select(`.${rowClassName}`)
                .append("svg")
                .attr("width", 100)
                .attr("height", 50);

            svg.append("text")
                .attr("x", 40)
                .attr("y", 20)
                .attr("text-anchor", "middle")
                .attr("fill", "#000")
                .style("font-size", "1rem")
                .text(`${data.contributionsTotal.text}/${data.goalsTotal.text}`);

            svg.append("rect")
                .attr("width", 80)
                .attr("height", 7)
                .attr("fill", "#c5d0e3")
                .attr("rx", 5)
                .attr("ry", 5)
                .attr("y", 30);

            svg.append("rect")
                .attr("width", percentage)
                .attr("height", 7)
                .attr("fill", "#6A87B8")
                .attr("rx", 5)
                .attr("ry", 5)
                .attr("y", 30);
        }
    }

    RenderIncomeByDateGraph(selector, incomeData) {
        document.querySelector(`${selector}`).innerHTML = '';
        
        let margin = {top: 20, right: 0, bottom: 30, left: 80};
        let width = 750 - margin.left - margin.right;
        let height = 300 - margin.top - margin.bottom;

        let svg = d3.select(selector)
            .append("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
            .append("g")
            .attr("transform", `translate(${margin.left},${margin.top})`);

        let x = d3.scaleUtc()
            .domain(d3.extent(incomeData, d => new Date(d.date.toString())))
            .range([0, width]);

        let y = d3.scaleLinear()
            .domain([0, d3.max(incomeData, d => d.total.amount)])
            .range([height, 0]);

        let area = d3.area()
            .x(d => x(new Date(d.date.toString())))
            .y0(height)
            .y1(d => y(d.total.amount))
            .curve(d3.curveCatmullRom);

        const line = d3.line()
            .x(d => x(new Date(d.date.toString())))
            .y(d => y(d.total.amount))
            .curve(d3.curveCatmullRom);

        // Define the gradient
        const gradient = svg.append("defs")
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

        svg.append("path")
            .datum(incomeData)
            .attr("class", "line")
            .attr("d", line)
            .style("fill", "none")
            .style("stroke", "#4F9B90")
            .style("stroke-width", 2);

        svg.append("path")
            .datum(incomeData)
            .attr("class", "area")
            .attr("d", area)
            .style("fill", "url(#areaGradient)");

        svg.append("g")
            .attr("transform", `translate(0,${height})`)
            .call(d3.axisBottom(x));

        svg.append("g")
            .call(d3.axisLeft(y));

        const tooltip = d3.select("body").append("div")
            .attr("class", "tooltip")
            .style("opacity", 0);

        svg.selectAll("circle")
            .data(incomeData)
            .enter().append("circle")
            .attr("r", 5)
            .attr("cx", d => x(new Date(d.date.toString())))
            .attr("cy", d => y(d.total.amount))
            .style("fill", "#4F9B90")
            .on("mouseover", function (event, d) {
                tooltip.transition()
                    .duration(200)
                    .style("opacity", .9);
                tooltip.html(`Date: ${new Date(d.date.toString()).toDateString()}<br/>This period: £${d.total.amount}k<br/>`)
                    .style("left", (event.pageX + 5) + "px")
                    .style("top", (event.pageY - 28) + "px");
            })
            .on("mouseout", function (d) {
                tooltip.transition()
                    .duration(500)
                    .style("opacity", 0);
            });

    }
    
    RenderFundraisersByCampaignGraph(selector, data) {
        let width = 380;
        let height = 270;
        let margin = {top: 30, right: 10, bottom: 20, left: 40};
        let radius = Math.min(width, height) / 2 - margin.top;

        let svg = d3.select(selector)
            .append("svg")
            .attr("width", width)
            .attr("height", height)
            .append("g")
            .attr("transform", `translate(${width / 2 + 80}, ${height / 2})`);

        let color = d3.scaleOrdinal()
            .domain(data.map(d => d.campaignName))
            .range(["#4F9B90", "#2152A3", "#507cc5", "#F5C1BC", "#FDD09FB2", "#FBE38EB2"]);

        let pie = d3.pie()
            .value(d => d.count)
            .sort(null);

        let arc = d3.arc()
            .innerRadius(radius * 0.5)
            .outerRadius(radius);

        const arcs = svg.selectAll(".arc")
            .data(pie(data))
            .enter()
            .append("g")
            .attr("class", "arc");

        arcs.append("path")
            .attr("d", arc)
            .attr("fill", d => color(d.data.campaignName));

        const legend = svg.append("g")
            .attr("transform", `translate(${-width / 2 - 70}, ${-height / 2 + 20})`);

        legend.selectAll("rect")
            .data(data)
            .enter()
            .append("rect")
            .attr("x", 0)
            .attr("y", (d, i) => i * 20)
            .attr("width", 18)
            .attr("height", 18)
            .attr("fill", d => color(d.campaignName));

        legend.selectAll("text")
            .data(data)
            .enter()
            .append("text")
            .attr("x", 24)
            .attr("y", (d, i) => i * 20 + 9)
            .attr("dy", "0.35em")
            .text(d => d.campaignName);
    }
}

angular.module('umbraco').factory('statisticsHelper', StatisticsHelper);