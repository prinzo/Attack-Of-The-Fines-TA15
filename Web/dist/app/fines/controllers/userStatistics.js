(function () {
    "use strict";
    angular
    .module("entelectFines")
    .controller("UserStatistics", ["dashboardResource", "$timeout", UserStatistics]);

    function UserStatistics(dashboardResource, $timeout) {
        var vm = this;
                 
        vm.chartSeriesDistribution = [
            {
                "name": "Fine Count",
                "data": [1, 2, 4, 1, 6, 5, 2, 1, 0, 0, 0, 0],
                "color": "green"
            }
        ];
            
        vm.chartConfigDistribution = {
            options: {
                chart: {
                    backgroundColor: "#f4f4f4",
                    plotShadow: true,
                    renderTo: 'userFinesLineGraph',
                    type: 'column'
                },

            },
            func: function (chart) {
                $timeout(function () {
                    chart.reflow();
                }, 0);
            },
            series: vm.chartSeriesDistribution,

            xAxis: {
                categories: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
                title: {
                    text: 'Months'
                }
            },
            yAxis: {
                title: {
                    text: 'Total Fines'
                }
            },
            title: {
                text: 'Fines for The Cat for 2015'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }
        

    }
}());
