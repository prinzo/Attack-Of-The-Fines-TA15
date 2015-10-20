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
            $rootScope.fines = data;
            console.log(data);
        },
                              
        function () {
            toaster.pop('error', "Fine Feed Failure", "No Fines were found");
        });

        vm.isOpen = false;


        vm.Second = function Second(Id) {
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
}());