(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("statisticsResource",
        [
            "$resource",
            "appSettings",
            statisticsResource
        ]);

    function statisticsResource($resource, appSettings) {
        return $resource(appSettings.serverPath + "/api/statistics/:action/:id");

    }
}());