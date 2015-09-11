(function () {
    "use strict";

    angular
        .module("common.services",
        ["ngResource"])
        .constant("appSettings",
        {
            serverPath: "http://http://localhost:56615/"
        });

}());