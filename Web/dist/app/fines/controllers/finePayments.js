(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("FinePayments", ['toaster',
                                   'finesResource',
                                   'userResource',
                                   'localStorageService',
                                   '$rootScope',
                                   FinaPayments]);

    function FinaPayments(toaster, finesResource, userResource, localStorageService, $rootScope) {
        var vm = this;
        
        $rootScope.checkUser();
        
        vm.users = userResource.query({
                    action: "GetAllUsers"
                }
            );
            
        vm.PayFine = function () {
             
            toaster.pop('success', "Fine Paid", "Fine paid successfully");
            
        }

        
    }
}());