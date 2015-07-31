(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Settings", ["userResource", "toaster", Settings]);

    function Settings(userResource, toaster) {
        var scope = this;
        scope.email = "prinay.panday@entelect.co.za";
        scope.user = [];
        scope.Name = '';
        scope.Surname = '';
        scope.UpdateUser = UpdateUser;

        GetUser();
        GetUserNameAndSurname();

        function GetUser() {
            var promise = userResource.get({
                action: "GetUserByEmail",
                email: scope.email
            });
            promise.$promise.then(function (data) {
                    scope.user = data;
                },
                function () {
                    toaster.error('Error', 'Failed to retrieve User Information');
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
                },
                function () {
                    toaster.error('Error', 'Failed to update User');
                });
        }

        function GetUserNameAndSurname() {
            var promise = userResource.query({
                action: "GetUserNameAndSurname",
                email: scope.email
            });
            promise.$promise.then(function (data) {
                    scope.Name = data[0];
                    scope.Surname = data[1];
                },
                function () {
                    toaster.error('Error', 'Failed to retrieve User Information');
                });
        }
    }
}());