(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("FineAwards", ['toaster',
                                   '$mdDialog',
                                   "$timeout",
                                   'finesResource',
                                   'userResource',
                                   'localStorageService',
                                   '$rootScope',
                                   '$q',
                                   FineAwards]);

    function FineAwards(toaster, $mdDialog, $timeout, finesResource, userResource, localStorageService, $rootScope, $q) {
        var vm = this;
        
        var currentUser = localStorageService.get('user'); 
        
        vm.selectedUser = [];
        vm.reason = "";

        vm.filterSelected = true;

        vm.users = userResource.query({
                    action: "GetAllUsers"
                }
            );

        function createFilterFor(query) {

          var lowercaseQuery = angular.lowercase(query);
          return function filterFn(contact) {

                if(!!contact && !!angular.lowercase(contact.DisplayName)) {
                    return ( angular.lowercase(contact.DisplayName).indexOf(lowercaseQuery) != -1);
                }

              return false;

          };
        }

        vm.Search = function(query) {var results = query ?
              vm.users.filter(createFilterFor(query)) : [];

            vm.selectedUser = [];
            return results;
        }

        vm.AwardFine = function () {
                        
            var newFineModel = {
                IssuerId: currentUser.Id,
                RecipientId: vm.selectedUser[0].Id,
                Reason: vm.reason
            };

            if(newFineModel.IssuerId == newFineModel.RecipientId) {
                toaster.pop('error', "Invalid Input", "You cannot award a fine for yourself");
            } else {
                var promise = finesResource.save({
                        action: "IssueFine"
                    },
                    newFineModel
                );

                promise.$promise.then(function (data) {
                        toaster.pop('success', "Fine Awarded", "Fine awarded successfully");


                        $timeout(function () {
                            var defer = $q.defer();

                            $mdDialog.hide();

                            $rootScope.fines.push(data);
                        });
                    },
                    function () {
                        toaster.error('Error', 'Failed to award fine');
                    });
            }
        }
        
    }
}());