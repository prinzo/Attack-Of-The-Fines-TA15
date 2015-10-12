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
        
        vm.filterSelected = true;
        
        vm.users = userResource.query({
                    action: "GetAllUsers"
                }
            );
              
        function createFilterFor(query) {
         
          var lowercaseQuery = angular.lowercase(query);
          return function filterFn(contact) {
                 return ( angular.lowercase(contact.DisplayName).indexOf(lowercaseQuery) != -1);
              
          };
        }
        
        vm.Search = function(query) {var results = query ?
              vm.users.filter(createFilterFor(query)) : [];
                                                 
            return results;
        }
        
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