(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("UserStatistics", ["dashboardResource",
                                   "$timeout",
                                   UserStatistics]);

    function UserStatistics(dashboardResource, $timeout) {
        var vm = this;

        vm.chartConfigDistribution = {
            chart: {
                height: 200
            },
            title: {
                text: 'Fines and Payments for the year',
                x: -20 //center
            },
            xAxis: {
                categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                title: {
                    text: 'Months'
                },
            },
            yAxis: {
                title: {
                    text: 'Total Fines'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                valueSuffix: '°C'
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            reflow: true,
            series: [{
                name: 'Fines',
                data: [-0.9, 0.6, 3.5, 8.4, 13.5, 17.0, 18.6, 17.9, 14.3, 9.0, 3.9, 1.0]
            }, {
                name: 'Payments',
                data: [3.9, 4.2, 5.7, 8.5, 11.9, 15.2, 17.0, 16.6, 14.2, 10.3, 6.6, 4.8]
            }]
        }

    }
}());