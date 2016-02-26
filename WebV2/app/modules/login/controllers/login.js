(function () {
    'use strict';
    angular
        .module('entelectFines.login')
        .controller('Login', ['UserService', 'AuthenticationService', 'SpinnerService', 'toaster', '$state', Login]);

    function Login(UserService, AuthenticationService, SpinnerService, toaster, $state) {
        var scope = this;
        $state.current.name = "login";
        scope.login = login;
        scope.username = '';
        scope.password = '';

        AuthenticationService.ClearCredentials();

        function login() {
            SpinnerService.StartSpin();
            var loginModel = {
                Username: scope.username,
                Password: scope.password
            };

            var promise = UserService.AuthenticateUser(loginModel);

            promise.$promise.then(function (data) {
                    SpinnerService.StopSpin();
                    AuthenticationService.SetCredentials(loginModel.Username, loginModel.Password, data.Id);
                    toaster.pop('success', 'Login Success', 'You should be working ' + data.Name);
                    $state.go('fines', {
                        userId: data.Id
                    });
                },
                function () {
                    SpinnerService.StopSpin();
                    toaster.pop('error', 'Login Failed', 'Incorrect credentials, please try again');
                });
        }

    }
}());