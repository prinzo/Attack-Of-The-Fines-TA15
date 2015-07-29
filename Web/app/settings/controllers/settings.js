(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Settings", ["userResource", Settings]);

    function Settings(userResource) {
        var scope = this;
        scope.email ="amrit.purshotam@entelect.co.za";
        
        scope.GetUser = GetUser;
        
        function GetUser() {
            var promise = userResource.get({
                action: "GetUserByEmail",
                email: scope.email
            });
            promise.$promise.then(function (data) {
                console.log(data);
            });
        }
    }
}());