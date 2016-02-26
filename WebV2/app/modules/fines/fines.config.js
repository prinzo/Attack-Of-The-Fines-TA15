(function () {
    'use strict';

    angular.module('entelectFines')
        .config(function ($stateProvider) {

            $stateProvider
                .state('fines', {
                    url: '/fines',
                    templateUrl: 'app/modules/fines/views/finesIndex.html'
                });
        });
})();