(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("FineAwards", ['toaster',
                                   'finesResource',
                                   'localStorageService',
                                   '$location',
                                   FineAwards]);

    function FineAwards(toaster, finesResource, localStorageService, $location) {
        var vm = this;
        if (localStorageService.get('user') == null) {
            $location.path("/Login");
        }
        vm.AwardFine = function () {

            var newFineModel = {
                IssuerId: '9f909fc1-405d-43ff-8b9d-381160892c61',
                RecipientId: 'e8f891fb-eff6-4787-bd02-636e5edd93b6',
                Reason: 'test'
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