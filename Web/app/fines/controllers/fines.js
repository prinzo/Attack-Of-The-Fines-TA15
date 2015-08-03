(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Fines", ['toaster', '$ngBootbox',
                              'localStorageService',
                              '$rootScope',
                              Fines]);

    function Fines(toaster, $ngBootbox, localStorageService, $rootScope) {
        var vm = this;
        
        $rootScope.checkUser();
        
        

        
    }
}());