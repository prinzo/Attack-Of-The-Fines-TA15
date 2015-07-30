(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Settings", ["userResource", "toaster", Settings]);

    function Settings(userResource, toaster) {
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
                    action: "UpdateUser"
                },
                userModel
            );

            promise.$promise.then(function (data) {
                toaster.pop('success', 'Update Successful', 'User was updated successfully');
                scope.user = data;
            });
        }
    }
}());