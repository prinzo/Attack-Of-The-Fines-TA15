(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Login", ["$location", "toaster", "localStorageService", Login]);

    function Login($location, toaster, localStorageService) {
        var vm = this;

        vm.loggedIn = false;
        vm.login = login;
        vm.username = "";
        vm.password = "";
        vm.correctName = "test@test.com";
        vm.correctPassword = "test";
        localStorageService.clearAll();

        function login() {

            if (vm.username === vm.correctName && vm.password === vm.correctPassword) {
                var user = localStorageService.get('user');

                if (user == null) {
                    user = {
                        username: vm.username,
                        password: vm.password
                    };
                }

                localStorageService.set('user', user);
                $location.path("/Fines");
                toaster.pop('success', "Login Success", "Successfully Logged In");

            } else {
                toaster.pop('error', "Login Failure", "Incorrect Login Details");
            }

        }

    }
}());