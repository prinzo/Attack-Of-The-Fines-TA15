(function () {
    "use strict";
    angular
        .module("entelectFines.login")
        .controller("Login", [Login]);

    function Login() {
        var scope = this;

        scope.login = login;
        scope.username = "";
        scope.password = "";

        function login() {

        }

    }
}());