(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Support", ["$rootScope"
                                , "supportResource"
                                , "toaster"
                                , "localStorageService"
                                , Support]);

    function Support($rootScope, supportResource, toaster, localStorageService) {

        $rootScope.checkUser();
        var scope = this;
        scope.tickets = {};

        GetUser();
        GetUserTickets();


        function GetUserTickets() {
            var promise = supportResource.query({
                action: "GetAllSupportTicketsForUser",
                userId: scope.user.Id
            });

            promise.$promise.then(function (data) {
                    scope.tickets = data;
                },
                function () {});
        }

        function GetUser() {
            scope.user = localStorageService.get('user');
        }
    }
}());