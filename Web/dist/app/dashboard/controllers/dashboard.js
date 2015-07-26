(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Dashboard", [Dashboard]);

    function Dashboard() {
        var vm = this;

        vm.chartTypes = [

            {
                "id": "bar",
                "title": "Bar"
            }
  ];

        vm.dashStyles = [
            {
                "id": "Solid",
                "title": "Solid"
            }
  ];

        vm.chartSeries = [
            {
                "name": "Fine Count",
                "data": [1, 2, 4, 7, 3],
                "color": "green"

            }
  ];

        vm.chartSeriesDistribution = [
            {
                "name": "Fine Count",
                "data": [1, 2, 4, 7, 3, 8, 12, 4, 6, 7, 22, 30],
                "color": "green"
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
                    backgroundColor: "#f4f4f4",
                    plotShadow: true,
                    renderTo: "panel",
                    type: 'bar'
                },

            },
            series: vm.chartSeries,
            xAxis: {
                categories: ['Prinay', 'Amrit', 'Kurt', 'Kristina', 'Kuben'],
                title: {
                    text: 'Employee'
                }
            },
            yAxis: {
                title: {
                    text: 'Number of Fines'
                }
            },
            title: {
                text: 'Leaderboard for 2015 - Top 5'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }

        vm.chartConfigDay = {
            options: {
                chart: {
                    backgroundColor: "#f4f4f4",
                    plotShadow: true,
                    type: 'bar'
                },

            },
            series: vm.chartSeries,
            xAxis: {
                categories: ['Prinay', 'Amrit', 'Kurt', 'Kristina', 'Kuben'],
                title: {
                    text: 'Employee'
                }
            },
            yAxis: {
                title: {
                    text: 'Number of Fines'
                }
            },
            title: {
                text: 'Leaderboard for Today - Top 5'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }

        vm.chartConfigDistribution = {
            options: {
                chart: {
                    backgroundColor: "#f4f4f4",
                    plotShadow: true,
                    renderTo: 'panel',
                    type: 'line'
                },

            },
            series: vm.chartSeriesDistribution,

            xAxis: {
                categories: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                title: {
                    text: 'Employee'
                }
            },
            yAxis: {
                title: {
                    text: 'Number of Fines'
                }
            },
            title: {
                text: 'Fine Distribution 2015'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }

    }
}());