(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Dashboard", [Dashboard]);

    function Dashboard() {
        var vm = this;

        vm.chartTypes = [
            {
                "id": "line",
                "title": "Line"
            },
            {
                "id": "spline",
                "title": "Smooth line"
            },
            {
                "id": "area",
                "title": "Area"
            },
            {
                "id": "areaspline",
                "title": "Smooth area"
            },
            {
                "id": "column",
                "title": "Column"
            },
            {
                "id": "bar",
                "title": "Bar"
            },
            {
                "id": "pie",
                "title": "Pie"
            },
            {
                "id": "scatter",
                "title": "Scatter"
            }
  ];

        vm.dashStyles = [
            {
                "id": "Solid",
                "title": "Solid"
            },
            {
                "id": "ShortDash",
                "title": "ShortDash"
            },
            {
                "id": "ShortDot",
                "title": "ShortDot"
            },
            {
                "id": "ShortDashDot",
                "title": "ShortDashDot"
            },
            {
                "id": "ShortDashDotDot",
                "title": "ShortDashDotDot"
            },
            {
                "id": "Dot",
                "title": "Dot"
            },
            {
                "id": "Dash",
                "title": "Dash"
            },
            {
                "id": "LongDash",
                "title": "LongDash"
            },
            {
                "id": "DashDot",
                "title": "DashDot"
            },
            {
                "id": "LongDashDot",
                "title": "LongDashDot"
            },
            {
                "id": "LongDashDotDot",
                "title": "LongDashDotDot"
            }
  ];

        vm.chartSeries = [
            {
                "name": "Some data",
                "data": [1, 2, 4, 7, 3]
            },
            {
                "name": "Some data 3",
                "data": [3, 1, null, 5, 2],
                connectNulls: true
            },
            {
                "name": "Some data 2",
                "data": [5, 2, 2, 3, 5],
                type: "column"
            },
            {
                "name": "My Super Column",
                "data": [1, 1, 2, 3, 2],
                type: "column"
            }
  ];

        vm.chartStack = [
            {
                "id": '',
                "title": "No"
            },
            {
                "id": "normal",
                "title": "Normal"
            },
            {
                "id": "percent",
                "title": "Percent"
            }
  ];

     
        vm.chartConfig = {
            options: {
                chart: {
                    type: 'areaspline'
                },
                plotOptions: {
                    series: {
                        stacking: ''
                    }
                }
            },
            series: vm.chartSeries,
            title: {
                text: 'Hello'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }

   
    }
}());