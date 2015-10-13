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
                                     '$scope',
                                   FinePayments]);

    function FinePayments(toaster, $ngBootbox, $timeout, finesResource, userResource, localStorageService, $rootScope, $q, $scope) {
        var vm = this;

        vm.selectedUser = [];
        
        $rootScope.checkUser();
        
        vm.users = userResource.query({
                    action: "GetAllUsers"
                }
            );
        
        var currentUser = localStorageService.get('user'); 
        
        vm.uploadButtonVisible = false;
        $scope.Image = '';

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

        
        var handleFileSelect = function (evt) {

            var file = evt.currentTarget.files[0];
            var reader = new FileReader();

            reader.onload = function (evt) {
                var img = new Image();
                img.src = evt.target.result;

                $scope.$apply(function (scope) {
                    $scope.Image = evt.target.result;
                });
            };

            reader.readAsDataURL(file);
            vm.showUpload = true;
            vm.showImage = true;

        };
        
        $timeout(function() {
            angular.element(document.querySelector('#paymentFileInput')).on('change', handleFileSelect);
        },1000, false);
        /*
        function uploadImage() {
            var payment = {
                Id: vm.user.Id,
                Id: vm.user.Id,
                Image: $scope.Image
            };
            
            var promise = userResource.save({
                    action: "UploadPaymentImage"
                },
                userModel
            );
            promise.$promise.then(function (data) {
                    toaster.pop('success', 'Update Successful', 'User image updated successfully');
                    vm.showUpload = false;
                    vm.showImage = false;
                    vm.user.Image = data.Image;
                    localStorageService.clearAll();
                    localStorageService.set('user', vm.selectedUser);
                },
                function () {
                    toaster.error('Error', 'Failed to update User image');
                });
        }*/
            
        vm.PayFine = function () {
             
            var newPaymentModel = {
                PayerId: currentUser.Id,
                RecipientId: vm.selectedUser[0].Id,
                TotalFinesPaid: vm.TotalFinesPaid,
                Image :  $scope.Image
            };

            var promise = finesResource.save({
                    action: "IssuePayment"
                },
                newPaymentModel
            );

            promise.$promise.then(function (data) {
                    toaster.pop('success', "Payment Awarded", "Payment awarded successfully");
                    
                    vm.showUpload = false;
                    vm.showImage = false;
                    localStorageService.clearAll();
                    localStorageService.set('user', vm.selectedUser);
                
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