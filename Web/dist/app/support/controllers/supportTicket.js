(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("SupportTicket", ["$rootScope"
                                , "supportResource"
                                , "toaster"
                                , "localStorageService"
                                , SupportTicket]);

    function SupportTicket($rootScope, supportResource, toaster, localStorageService) {

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
            var supportTicket = {
                Subject: scope.subject,
                Message: scope.message,
                Type : scope.type
            };

            console.log(supportTicket);
        }

    }
}());