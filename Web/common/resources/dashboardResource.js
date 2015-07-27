(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("dashboardResource",
        [
            "$resource",
            "appSettings",
            dashboardResource
        ]);

    function dashboardResource($resource, appSettings) {
        return $resource(appSettings.serverPath + "/api/dashboard/:action/:id");

    }
}());