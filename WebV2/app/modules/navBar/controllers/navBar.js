(function () {
    'use strict';
    angular
        .module('entelectFines')
        .controller("NavBar", ['AuthenticationService', '$state', "toaster", NavBar]);

    function NavBar(AuthenticationService, $state, toaster) {
        var scope = this;
        scope.logout = logout;


        function logout() {
            toaster.pop('success', "Session Terminated", "Get back to work, I'm watching you");

            AuthenticationService.ClearCredentials();

            $state.go('login', {});
        }
    }
}());