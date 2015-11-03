var app = angular.module("entelectFines", ["common.services",
                         "ngRoute",
                         "toaster",
                          "highcharts-ng",
                          "ngBootbox",
                          "LocalStorageModule",
                          "angularFileUpload",
                           "angucomplete-alt",
                           "angularSpinner",
                           "ngAnimate",
                           "ngAria",
                           "ngResource",
                           "ngMaterial"
                        ]);



app.config(['$routeProvider',
 function ($routeProvider) {
            $routeProvider.

            when('/Dashboard', {
                templateUrl: 'app/dashboard/views/dashboardView.html',
                controller: ''
            }).
            otherwise({
                redirectTo: '/Dashboard'
            });

 }])
    .config(['localStorageServiceProvider',
            function (localStorageServiceProvider) {
            localStorageServiceProvider.setPrefix('localUser');
}]);

