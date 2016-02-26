(function () {
    'use strict';
    angular
        .module('entelectFines.login')
        .controller('Login', ['UserService', 'AuthenticationService', 'toaster', '$state', Login]);

    function Login(UserService, AuthenticationService, toaster, $state) {
        var scope = this;
        $state.current.name = 'login';
        scope.login = login;
        scope.username = '';
        scope.password = '';

        function login() {
            var loginModel = {
                Username: scope.username,
                Password: scope.password
            };

            var promise = UserService.AuthenticateUser(loginModel);

            promise.$promise.then(function (data) {
                    AuthenticationService.SetCredentials(loginModel.Username, loginModel.Password, data.Id);
                    toaster.pop('success', 'Login Success', 'Welcome back ' + data.Name);
                    $state.go('fines', {
                        userId: data.Id
                    });
                },
                function () {
                    toaster.pop('error', 'Login Failed', 'Incorrect credentials, please try again');
                });
        }

    }
}());