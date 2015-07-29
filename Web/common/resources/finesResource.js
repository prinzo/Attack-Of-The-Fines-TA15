(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("finesResource",
        [
            "$resource",
            "appSettings",
            finesResource
        ]);

    function finesResource($resource, appSettings) {
        return $resource(appSettings.serverPath + "/api/fines/IssueFine");

    }
}());
