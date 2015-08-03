(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("FineAwards", ['toaster',
                                   'finesResource',
                                   'userResource',
                                   'localStorageService',
                                   '$rootScope',
                                   FineAwards]);

    function FineAwards(toaster, finesResource, userResource, localStorageService, $rootScope) {
        var vm = this;
        
        $rootScope.checkUser();
        
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
                },  
                function () {
                    toaster.error('Error', 'Failed to award fine');
                });
        }

        
    }
}());