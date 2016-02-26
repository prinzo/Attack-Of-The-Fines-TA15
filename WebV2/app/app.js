var app = angular.module('entelectFines', ['entelectFines.login',
    'ui.router',
    'entelectFines.api',
    'entelectFines.fines',
    'ngCookies',
    'toaster',
    'angularSpinner',
    'angucomplete-alt',
    'ngAnimate',
    'ngAria',

    'ngMaterial'
    ]);

app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/login');

});

app.run(['$rootScope', '$cookieStore', '$state', '$http', 'toaster',
    function ($rootScope, $cookieStore, $state, $http, toaster) {

        $rootScope.globals = $cookieStore.get('globals') || {};

        if ($rootScope.globals.currentUser) {
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata; // jshint ignore:line
        }


        $rootScope.$on('$stateChangeStart', function (event, next, current) {

            if ($state.current.name != 'login') {

                if (!$rootScope.globals.currentUser) {
                    event.preventDefault();

                    $state.current.name = 'login';

                    $state.go('login', {});
                }

            }
        });
    }]);