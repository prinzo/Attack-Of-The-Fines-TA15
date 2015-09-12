(function () {
    "use strict";

    angular
        .module("common.services")
        .factory("supportResource",
        [
            "$resource",
            "appSettings",
            supportResource
        ]);

    function supportResource($resource, appSettings) {
         return $resource(appSettings.serverPath + "/api/support/:action/:id");

    }
}());