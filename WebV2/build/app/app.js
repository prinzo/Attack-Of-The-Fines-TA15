var app = angular.module("entelectFines", ['entelectFines.login',
    'ui.router']);

app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise("/login");

});
(function () {
    'use strict';

    angular.module('entelectFines')
        .config(function ($stateProvider) {

            $stateProvider
                .state('login', {
                    url: '/login',
                    templateUrl: 'app/modules/login/views/login.html'
                });
        });
})();
(function () {
    'use strict';

    angular.module('entelectFines.login', []);

})();
(function () {
    "use strict";
    angular
        .module("entelectFines.login")
        .controller("Login", [Login]);

    function Login() {
        var scope = this;

        scope.login = login;
        scope.username = "";
        scope.password = "";

        function login() {

        }

    }
}());