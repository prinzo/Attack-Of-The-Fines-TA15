(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Fines", ['toaster', '$ngBootbox',
                              'localStorageService',
                              'finesResource',
                              '$rootScope',
                              'userResource',
                              '$timeout',
                              Fines]);

    function Fines(toaster, $ngBootbox, localStorageService, finesResource, $rootScope, userResource, $timeout) {
        var vm = this;
        
         vm.dialogOptions = {
                            $scope : vm
                       }
         
        $rootScope.checkUser();
        
        $rootScope.fines = {};
        
        var promise = finesResource.query({
                    action: "GetFines"
                }
            );
        
        promise.$promise.then(function (data) {
                        
            for(var d = 0; d < data.length; d++) {
                toaster.pop('success', "got fine", "fine is: " + data[d].ReceiverDisplayName);
            }
            
            $rootScope.fines = data;
        },
                              
        function () {
            toaster.pop('error', "Fine Feed Failure", "No Fines were found");
        });
        
        
        
    }
}());