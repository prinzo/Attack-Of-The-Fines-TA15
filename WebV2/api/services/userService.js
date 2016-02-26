'use strict';

angular.module('entelectFines.api')

    .factory('UserService', ['userResource',
        function (userResource) {
            var service = {};
            service.AuthenticateUser = AuthenticateUser;

            function AuthenticateUser(loginModel) {
                var promise = userResource.save({
                        action: "AuthenticateUser"
                    },
                    loginModel
                );

                return promise;
            }

            return service;
        }])