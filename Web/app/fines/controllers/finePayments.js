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
        
        $rootScope.checkUser();
        
        vm.users = userResource.query({
                    action: "GetAllUsers"
                }
            );
        
        var currentUser = localStorageService.get('user'); 
        
        vm.uploadButtonVisible = false;
        $scope.Image = '';
        
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
        
       /* function uploadImage() {
            var payment = {
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
                    localStorageService.set('user', scope.user);
                },
                function () {
                    toaster.error('Error', 'Failed to update User image');
                });
        }*/
            
        vm.PayFine = function () {
             
            var newPaymentModel = {
                PayerId: currentUser.Id,
                RecipientId: vm.selectedUser["originalObject"].Id,
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
                    localStorageService.set('user', scope.user);
                
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