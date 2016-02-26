'use strict';

angular.module('entelectFines.api')

    .factory('SpinnerService', ['usSpinnerService',
        function (usSpinnerService) {
            var service = {};
            service.StartSpin = StartSpin;
            service.StopSpin = StopSpin;

            function StartSpin() {
                usSpinnerService.spin('spinner-1');
            }

            function StopSpin() {
                usSpinnerService.stop('spinner-1');
            }

            return service;
        }])