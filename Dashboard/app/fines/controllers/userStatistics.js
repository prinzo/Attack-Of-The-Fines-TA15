(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("UserStatistics", ["userResource",
                                   "toaster",
                                    "$rootScope",
                                   UserStatistics]);

    function UserStatistics(userResource, toaster, $rootScope) {
        var vm = this;

            var promise = userResource.get({
                action: "GetStatisticsForUser",
                id: $rootScope.userId
            });

            promise.$promise.then(function (data) {
                    $("#userName").text(data.UserDisplayName);
                    $("#totalFinesEver").text(data.TotalFinesEver);
                    $("#totalFinesForMonth").text(data.TotalFinesForMonth);
                    $("#totalPaymentsEver").text(data.TotalPaymentsEver);
                    $("#totalPaymentsForMonth").text(data.TotalPaymentsForMonth);

                    drawChart(data);

                    $(".statisticsModal").removeClass('hidden');
                },

                function () {
                    toaster.pop('error', "User Statistics Failure", "The statistics for this user could not be retrieved");
                });

        function drawChart(data) {
            vm.chartConfigDistribution = {
                chart: {
                    height: 100
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
                    data: data.DisplayableFinesForYear
                }, {
                    name: 'Payments',
                    data: data.DisplayablePaymentsForYear
                }]
            }
        }
    }
}());