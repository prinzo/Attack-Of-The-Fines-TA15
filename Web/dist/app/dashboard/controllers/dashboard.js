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
                "data": [1, 2, 4, 7, 3]
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
                text: 'Leaderboard for 2015'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }

   
    }
}());