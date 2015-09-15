(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Fines", ['toaster', '$ngBootbox',
                              'localStorageService',
                              'finesResource',
                              '$rootScope',
                              Fines]);

    function Fines(toaster, $ngBootbox, localStorageService, finesResource, $rootScope) {
        var vm = this;
        
        $rootScope.checkUser();
        
        vm.fines = {};
        
        var promise = finesResource.query({
                    action: "GetFines"
                }
            );
        
        promise.$promise.then(function (data) {
                        
            for(var d = 0; d < data.length; d++) {
                toaster.pop('success', "got fine", "fine is: " + data[d].ReceiverDisplayName);
            }
            
            vm.fines = data;
        },
                              
        function () {
            toaster.pop('error', "Fine Feed Failure", "No Fines were found");
        });
        
    }
}());