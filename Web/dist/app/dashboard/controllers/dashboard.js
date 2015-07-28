(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Dashboard", ["dashboardResource", "$timeout", "$interval", "$window", Dashboard]);

    function Dashboard(dashboardResource, $timeout, $interval, $window) {
        var vm = this;
        var seriesData = [];
        var categories = [];
        var seriesToday = [];
        var categoriesToday = [];

        GetOverallLeaderboard();
        GetLeaderboardToday();

        $interval(GetOverallLeaderboard, 60000);
        $interval(GetLeaderboardToday, 60000);


     
        function GetOverallLeaderboard() {
            seriesData.length = 0;
            categories.length = 0;
            var promise = dashboardResource.query({
                action: "GetLeaderboard"
            });
            promise.$promise.then(function (data) {

                for (var j = 0; j < data.length; j++) {
                    seriesData.push(data[j].AwardedFineCount);
                    categories.push(data[j].EmailAddress);
                }
            });
        }


        function GetLeaderboardToday() {
              seriesToday.length = 0;
            categoriesToday.length = 0;
            var promise = dashboardResource.query({
                action: "GetLeaderboardToday"
            });
            promise.$promise.then(function (data) {
                for (var j = 0; j < data.length; j++) {
                    seriesToday.push(data[j].AwardedFineCount);
                    categoriesToday.push(data[j].EmailAddress);
                }
            });
        }

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
                "data": seriesData,
                "color": "green"

            }
  ];

        vm.chartSeriesToday = [
            {
                "name": "Fine Count",
                "data": seriesToday,
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
                func: function (chart) {
                    $timeout(function () {
                        chart.reflow();
                    }, 0);
                },

            },
            series: vm.chartSeries,
            xAxis: {
                categories: categories,
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
                func: function (chart) {
                    $timeout(function () {
                        chart.reflow();
                    }, 0);
                },

            },
            series: vm.chartSeriesToday,
            xAxis: {
                categories: categoriesToday,
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
            func: function (chart) {
                $timeout(function () {
                    chart.reflow();
                }, 0);
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