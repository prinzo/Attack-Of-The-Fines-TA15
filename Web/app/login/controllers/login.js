(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Login", ["$location", "toaster", Login]);

    function Login($location, toaster) {
        var vm = this;

        vm.loggedIn = false;
        vm.login = login;
        vm.username = "";
        vm.password = "";
        vm.correctName = "test@test.com";
        vm.correctPassword = "test";

        function login() {
            if (vm.username === vm.correctName && vm.password === vm.correctPassword) {
                vm.loggedIn = true;
                console.log(vm.loggedIn);
                $location.path("/Fines");
                toaster.pop('success', "Login Success", "Successfully Logged In");

            } else {
                toaster.pop('error', "Login Failure", "Incorrect Login Details");
            }

        }

    }
}());