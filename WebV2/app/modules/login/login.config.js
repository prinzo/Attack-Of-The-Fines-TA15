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