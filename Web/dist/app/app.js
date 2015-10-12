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
        when('/Fines', {
            templateUrl: 'app/fines/views/finesView.html',
            controller: ''
        }).
        when('/Users', {
            templateUrl: 'app/users/views/usersView.html',
            controller: ''
        }).
        when('/Dashboard', {
            templateUrl: 'app/dashboard/views/dashboardView.html',
            controller: ''
        }).
        when('/Settings', {
            templateUrl: 'app/settings/views/settingsView.html',
            controller: ''
        }).
        when('/Support', {
            templateUrl: 'app/support/views/supportView.html',
            controller: ''
        }).
        when('/Login', {
            templateUrl: 'app/login/views/loginView.html',
            controller: ''
        }).
           when('/CreateSupportTicket', {
            templateUrl: 'app/support/views/supportTicketView.html',
            controller: ''
        }).
        otherwise({
            redirectTo: '/CreateSupportTicket'
        });

 }])
    .config(['localStorageServiceProvider',
            function (localStorageServiceProvider) {
            localStorageServiceProvider.setPrefix('localUser');
}]);

app.run(function ($rootScope, localStorageService, $location) {
    $rootScope.checkUser = function () {
        if (localStorageService.get('user') == null) {
            $location.path("/Login");
        }
    }

})