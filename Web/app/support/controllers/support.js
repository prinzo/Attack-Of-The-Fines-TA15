(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Support", ['$rootScope',
                 Support]);

    function Support($rootScope) {
        $rootScope.checkUser();
        var scope = this;
        
    }
}());