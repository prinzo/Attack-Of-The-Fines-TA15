(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Login", ["$location",
                              "toaster",
                              "localStorageService",
                              "userResource",
                              "usSpinnerService",
                              Login]);

    function Login($location, toaster, localStorageService, userResource,usSpinnerService) {
        var vm = this;
        localStorageService.clearAll();
        if (localStorageService.get('user') == null) {
            toaster.error('User Not Signed In', 'Please Login To Access Site');
        }
        vm.loggedIn = false;
        vm.login = login;
        vm.username = "";
        vm.password = "";
        
        function login() {
            startSpin();
            var promise = userResource.get({
                action: "AuthenticateUser",
                domainName: vm.username,
                password: vm.password
            });
            promise.$promise.then(function (data) {
                    var user = localStorageService.get('user');
                    user = data;
                    localStorageService.set('user', user);
                    $location.path("/Fines");
                    toaster.pop('success', "Login Success", "Successfully Logged In");
                },
                function () {
                    toaster.pop('error', "Login Failure", "Incorrect Login Details");
                });
        }
        
        function startSpin(){
        usSpinnerService.spin('spinner-1');
        }
    }
}());