(function () {
    'use strict';

    angular
        .module('entelectFines.api')
        .factory('userResource',
            [
                '$resource',
                'appSettings',
                userResource
            ]);

    function userResource($resource, appSettings) {
        return $resource(appSettings.serverPath + '/api/user/:action/:id');

    }
}());