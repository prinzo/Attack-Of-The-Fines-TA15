(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("FinePayments", ['toaster',
                                   '$ngBootbox',
                                   "$timeout",
                                   'finesResource',
                                   'userResource',
                                   'localStorageService',
                                   '$rootScope',
                                   '$q',
                                   FinePayments]);

    function FinePayments(toaster, $ngBootbox, $timeout, finesResource, userResource, localStorageService, $rootScope, $q) {
        var vm = this;
        
        $rootScope.checkUser();
        
        vm.users = userResource.query({
                    action: "GetAllUsers"
                }
            );
        
        var currentUser = localStorageService.get('user'); 
            
        vm.PayFine = function () {
             
            var newPaymentModel = {
                PayerId: currentUser.Id,
                RecipientId: vm.selectedUser["originalObject"].Id,
                TotalFinesPaid: vm.TotalFinesPaid,
                Image : null
            };

            var promise = finesResource.save({
                    action: "IssuePayment"
                },
                newPaymentModel
            );

            promise.$promise.then(function (data) {
                    toaster.pop('success', "Payment Awarded", "Payment awarded successfully");
                    
                
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