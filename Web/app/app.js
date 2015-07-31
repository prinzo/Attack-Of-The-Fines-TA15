var app = angular.module("entelectFines", ["common.services", "ngRoute", "toaster", "highcharts-ng", "ngBootbox", "LocalStorageModule"]);



app.config(['$routeProvider',
 function ($routeProvider) {
        $routeProvider.
        when('/Fines', {
            templateUrl: 'app/fines/views/finesView.html',
            controller: 'Fines'
        }).
        when('/Users', {
            templateUrl: 'app/users/views/usersView.html',
            controller: 'Users'
        }).
        when('/Dashboard', {
            templateUrl: 'app/dashboard/views/dashboardView.html',
            controller: 'Dashboard'
        }).
        when('/Settings', {
            templateUrl: 'app/settings/views/settingsView.html',
            controller: 'Settings'
        }).
        when('/Support', {
            templateUrl: 'app/support/views/supportView.html',
            controller: 'Support'
        }).
        when('/Login', {
            templateUrl: 'app/login/views/loginView.html',
            controller: 'Login'
        }).
        otherwise({
            redirectTo: '/Login'
        });

 }])
    .config(['localStorageServiceProvider',
            function (localStorageServiceProvider) {
                localStorageServiceProvider.setPrefix('localUser');
}]);