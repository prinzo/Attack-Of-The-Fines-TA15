(function () {
    "use strict";
    angular
        .module("entelectFines")
        .filter("CapitalFilter", [CapitalFilter]);

    function CapitalFilter() {
        return function (input) {
            return input.charAt(0).toUpperCase() + input.substr(1).toLowerCase();
        }

    }
}());