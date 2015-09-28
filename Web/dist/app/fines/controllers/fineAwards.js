(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("FineAwards", ['toaster',
                                   '$ngBootbox',
                                   "$timeout",
                                   'finesResource',
                                   'userResource',
                                   'localStorageService',
                                   '$rootScope',
                                   '$q',
                                   FineAwards]);

    function FineAwards(toaster, $ngBootbox, $timeout, finesResource, userResource, localStorageService, $rootScope, $q) {
        var vm = this;
        
        var currentUser = localStorageService.get('user'); 
        
        vm.selectedUser;
        vm.reason = "";
        
        vm.users = userResource.query({
                    action: "GetAllUsers"
                }
            );
                     
        vm.AwardFine = function () {
                        
            var newFineModel = {
                IssuerId: currentUser.Id,
                RecipientId: vm.selectedUser["originalObject"].Id,
                Reason: vm.reason
            };

            var promise = finesResource.save({
                    action: "IssueFine"
                },
                newFineModel
            );

            promise.$promise.then(function (data) {
                    toaster.pop('success', "Fine Awarded", "Fine awarded successfully");
                    
                
                    $timeout(function(){
                       var defer = $q.defer();
                    
                       $ngBootbox.hideAll();
                        
                        $rootScope.fines.push(data);
                    });
                },  
                function () {
                    toaster.error('Error', 'Failed to award fine');
                });
        }
        
    }
}());