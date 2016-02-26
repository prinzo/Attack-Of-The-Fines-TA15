(function () {
    'use strict';

    angular
        .module('entelectFines.api')
        .factory('finesResource',
            [
                '$resource',
                'appSettings',
                finesResource
            ]);

    function finesResource($resource, appSettings) {
        return $resource(appSettings.serverPath + '/api/fines/:action/:id');

    }
}());