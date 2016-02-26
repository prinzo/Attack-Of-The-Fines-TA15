(function () {
    "use strict";

    angular
        .module("entelectFines.api", ["ngResource"])
        .constant("appSettings", {
            serverPath: "http://entelect.finesapi.local"
        });

}());