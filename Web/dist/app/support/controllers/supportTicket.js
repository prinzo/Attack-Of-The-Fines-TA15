(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("SupportTicket", ["$rootScope"
                                , "supportResource"
                                , "toaster"
                                , "localStorageService"
                                , "$location"
                                , "usSpinnerService"
                                , SupportTicket]);

    function SupportTicket($rootScope, supportResource, toaster, localStorageService, $location, usSpinnerService) {

        $rootScope.checkUser();
        var scope = this;
        scope.subject = "";
        scope.message = "";
        scope.type = 0;
        scope.CreateSupportTicket = CreateSupportTicket;

        GetUser();

        function GetUser() {
            scope.user = localStorageService.get('user');
        }

        function CreateSupportTicket() {
            startSpin();
            var supportTicket = {
                Subject: scope.subject,
                Message: scope.message,
                Type: scope.type,
                UserId: scope.user.Id
            };

            var promise = supportResource.save({
                    action: "CreateSupportTicket"
                },
                supportTicket);

            promise.$promise.then(function (data) {
                    toaster.pop('success', "Ticket Created", "Successfully created support ticket");
                    $location.path("/Support");
                },
                function () {
                    stopSpin();
                    toaster.pop('error', "Ticket Error", "Creation of support ticket failed");

                });

        }

        function startSpin() {
            usSpinnerService.spin('spinner-1');
        }

        function stopSpin() {
            usSpinnerService.stop('spinner-1');
        }

    }
}());