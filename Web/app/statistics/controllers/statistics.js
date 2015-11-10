(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Statistics", ["statisticsResource",
                                  "$timeout",
                                  "$interval",
                                  "$window",
                                  "localStorageService",
                                  "$rootScope",
                                  Statistics]);

    function Statistics(statisticsResource, $timeout, $interval, $window, localStorageService, $rootScope) {
        var vm = this;
        $rootScope.checkUser();
        var seriesData = [];
        var categories = [];
        var seriesToday = [];
        var categoriesToday = [];
        var seriesWeek = [];
        var categoriesWeek = [];
        var seriesMonth = [];
        var categoriesMonth = [];
        vm.DownloadAllFines = DownloadAllFines;
        GetOverallLeaderboard();
        GetLeaderboardToday();
        GetLeaderboardWeek();
        GetLeaderboardMonth();

        $interval(GetOverallLeaderboard, 60000);
        $interval(GetLeaderboardToday, 60000);
        $interval(GetLeaderboardWeek, 60000);
        $interval(GetLeaderboardMonth, 60000);


        function DownloadAllFines() {
            var form = document.createElement("form");
            form.setAttribute("action", "http://entelect.finesapi.local/api/Fines/ExportAllFines");
            form.setAttribute("method", "get");
            form.setAttribute("target", "_blank");

            var hiddenEle1 = document.createElement("input");
            hiddenEle1.setAttribute("type", "hidden");
            hiddenEle1.setAttribute("name", "some");
            hiddenEle1.setAttribute("value", "value");


            form.submit();
        }

        function GetOverallLeaderboard() {
            seriesData.length = 0;
            categories.length = 0;
            var promise = statisticsResource.query({
                action: "GetLeaderboard"
            });
            promise.$promise.then(function (data) {

                for (var j = 0; j < data.length; j++) {
                    seriesData.push(data[j].AwardedFineCount);
                    categories.push(data[j].DisplayName);
                }
            });
        }

        function GetLeaderboardMonth() {
            seriesMonth.length = 0;
            categoriesMonth.length = 0;
            var promise = statisticsResource.query({
                action: "GetLeaderboardForMonth"
            });
            promise.$promise.then(function (data) {

                for (var j = 0; j < data.length; j++) {
                    seriesMonth.push(data[j].AwardedFineCount);
                    categoriesMonth.push(data[j].DisplayName);
                }
            });
        }

        function GetLeaderboardWeek() {
            seriesWeek.length = 0;
            categoriesWeek.length = 0;
            var promise = statisticsResource.query({
                action: "GetLeaderboardForWeek"
            });
            promise.$promise.then(function (data) {

                for (var j = 0; j < data.length; j++) {
                    seriesWeek.push(data[j].AwardedFineCount);
                    categoriesWeek.push(data[j].DisplayName);
                }
            });
        }

        function GetLeaderboardToday() {
            seriesToday.length = 0;
            categoriesToday.length = 0;
            var promise = statisticsResource.query({
                action: "GetLeaderboardToday"
            });
            promise.$promise.then(function (data) {
                for (var j = 0; j < data.length; j++) {
                    seriesToday.push(data[j].AwardedFineCount);
                    categoriesToday.push(data[j].DisplayName);
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

        vm.chartSeriesWeek = [
            {
                "name": "Fine Count",
                "data": seriesWeek,
                "color": "green"

            }
  ];
        vm.chartSeriesMonth = [
            {
                "name": "Fine Count",
                "data": seriesMonth,
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
                    backgroundColor: "#FFFFFF",
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
                text: 'Yearly Leaderboard- Top 5'
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
                    backgroundColor: "#FFFFFF",
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
                text: 'Daily Leaderboard - Top 5'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }

        vm.chartConfigWeek = {
            options: {
                chart: {
                    backgroundColor: "#FFFFFF",
                    plotShadow: true,
                    type: 'bar'
                },
                func: function (chart) {
                    $timeout(function () {
                        chart.reflow();
                    }, 0);
                },

            },
            series: vm.chartSeriesWeek,
            xAxis: {
                categories: categoriesWeek,
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
                text: 'Weekly Leaderboard - Top 5'
            },
            credits: {
                enabled: true
            },
            loading: false,
            size: {}
        }

        vm.chartConfigMonth = {
            options: {
                chart: {
                    backgroundColor: "#FFFFFF",
                    plotShadow: true,
                    type: 'bar'
                },
                func: function (chart) {
                    $timeout(function () {
                        chart.reflow();
                    }, 0);
                },

            },
            series: vm.chartSeriesMonth,
            xAxis: {
                categories: categoriesMonth,
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
                text: 'Monthly Leaderboard - Top 5'
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
                    backgroundColor: "#FFFFFF",
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