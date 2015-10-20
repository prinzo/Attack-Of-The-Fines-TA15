(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Fines", ['toaster',
                              'localStorageService',
                              'finesResource',
                              '$rootScope',
                                '$mdDialog',
                              Fines]);

    function Fines(toaster, localStorageService, finesResource, $rootScope,$mdDialog) {
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
            $rootScope.fines = data;
            console.log(data);
        },
                              
        function () {
            toaster.pop('error', "Fine Feed Failure", "No Fines were found");
        });

        vm.isOpen = false;

        vm.ShowAlert = function(ev) {
            var seconder = $("#seconder").val();
            $mdDialog.show(
                $mdDialog.alert()
                    .parent(angular.element(document.querySelector('#fines-container')))
                    .clickOutsideToClose(true)
                    .title('Fine has been seconded')
                    .content('This fine has already been seconded by ' + seconder)
                    .ariaLabel('Fine has been seconded')
                    .ok('OK')
                    .targetEvent(ev)
            );
        }

        vm.Second = function Second(Id, event) {
            var isSeconded = $("#isSeconded").val();

            if(isSeconded) {
                vm.ShowAlert(event);
            } else {
                var secondFineModel = {
                    UserId: localStorageService.get('user').Id,
                    FineId: Id
                };

                var promise = finesResource.save({
                        action: "SecondFine"
                    },
                    secondFineModel
                );

                promise.$promise.then(function (data) {
                        toaster.pop('success', "Seconded", "Seconded");
                        $(".buttonSecond" + Id).removeClass('fa fa-angellist');
                        $(".buttonSecond" + Id).addClass('glyphicon glyphicon-ok');
                    },

                    function () {
                        toaster.pop('error', "Fine Feed Failure", "No Fines were found");
                    });
            }
        }

    }
}());