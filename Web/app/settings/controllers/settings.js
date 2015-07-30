(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Settings", ["userResource", Settings]);

    function Settings(userResource) {
        var scope = this;

        scope.email = "amrit.purshotam@entelect.co.za";
        scope.user = [];

        GetUser();
        scope.UpdateUser = UpdateUser;

        function GetUser() {
            var promise = userResource.get({
                action: "GetUserByEmail",
                email: scope.email
            });
            promise.$promise.then(function (data) {
                scope.user = data;
            });
        }

        function UpdateUser() {

            var userModel = {
                Id: scope.user.Id,
                SlackId: scope.user.SlackId,
                EmailAddress: scope.user.EmailAddress,
                DisplayName: scope.user.DisplayName,
                AwardedFineCount: scope.user.AwardedFineCount,
                PendingFineCount: scope.user.PendingFineCount,
                Fines: null
            };


            var promise = userResource.save({
                action: "UpdateUser",
                userModel: userModel
            });

            promise.$promise.then(function (data) {
                console.log("Updated User");
                scope.user = data;
            });
        }
    }
}());