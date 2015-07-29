(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Settings", ["userResource", Settings]);

    function Settings(userResource) {
        var scope = this;
        scope.email = "amrit.purshotam@entelect.co.za";
        scope.user = [];
        scope.DisplayName = "";

        scope.GetUser = GetUser();
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
            var promise = userResource.save({
                action: "UpdateUser",
                userModel: scope.user
            });
                console.log(promise);
            promise.$promise.then(function (data) {
                scope.user = data;
                console.log("Updated");
            });

        }
    }
}());