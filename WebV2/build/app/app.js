var app = angular.module('entelectFines', ['entelectFines.login',
    'ui.router',
    'entelectFines.api',
    'entelectFines.fines',
    'ngCookies',
    'toaster'
    ]);

app.config(function ($stateProvider, $urlRouterProvider) {
    $urlRouterProvider.otherwise('/login');

});

app.run(['$rootScope', '$cookieStore', '$state', '$http', 'toaster',
    function ($rootScope, $cookieStore, $state, $http, toaster) {

        $rootScope.globals = $cookieStore.get('globals') || {};

        if ($rootScope.globals.currentUser) {
            $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata; // jshint ignore:line
        }


        $rootScope.$on('$stateChangeStart', function (event, next, current) {

            if ($state.current.name !== 'login') {

                if (!$rootScope.globals.currentUser) {
                    event.preventDefault();
                    toaster.pop('error', 'User Not Signed In', 'Please login to access the portal');

                    $state.current.name = 'login';

                    $state.go('login', {});
                }

            }
        });
    }]);
(function () {
    'use strict';

    angular.module('entelectFines')
        .config(function ($stateProvider) {

            $stateProvider
                .state('login', {
                    url: '/login',
                    templateUrl: 'app/modules/login/views/login.html'
                });
        });
})();
(function () {
    'use strict';

    angular.module('entelectFines.login', []);

})();
(function () {
    'use strict';

    angular.module('entelectFines')
        .config(function ($stateProvider) {

            $stateProvider
                .state('fines', {
                    url: '/fines',
                    templateUrl: 'app/modules/fines/views/finesIndex.html'
                });
        });
})();
(function () {
    'use strict';

    angular.module('entelectFines.fines', []);

})();
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
(function () {
    'use strict';
    angular
        .module('entelectFines.fines')
        .controller('FinesIndex', [FinesIndex]);

    function FinesIndex() {
        var scope = this;

    }
}());
(function () {
    "use strict";

    angular
        .module("entelectFines.api", ["ngResource"])
        .constant("appSettings", {
            serverPath: "http://entelect.finesapi.local"
        });

}());
(function () {
    "use strict";

    angular
        .module("entelectFines.api")
        .factory("userResource",
            [
                "$resource",
                "appSettings",
                userResource
            ]);

    function userResource($resource, appSettings) {
        return $resource(appSettings.serverPath + "/api/user/:action/:id");

    }
}());
'use strict';

angular.module('entelectFines.api')

    .factory('AuthenticationService', ['Base64', '$http', '$cookieStore', '$rootScope',
        function (Base64, $http, $cookieStore, $rootScope) {
            var service = {};

            service.SetCredentials = function (username, password, id) {
                var authdata = Base64.encode(username + ':' + password);

                $rootScope.globals = {
                    currentUser: {
                        username: username,
                        authdata: authdata,
                        id: id
                    }
                };

                $http.defaults.headers.common['Authorization'] = 'Basic ' + authdata;
                $cookieStore.put('globals', $rootScope.globals);
            };

            service.ClearCredentials = function () {
                $rootScope.globals = {};
                $cookieStore.remove('globals');
                $http.defaults.headers.common.Authorization = 'Basic ';
            };

            return service;
        }])

    .factory('Base64', function () {
        /* jshint ignore:start */

        var keyStr = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=';

        return {
            encode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                do {
                    chr1 = input.charCodeAt(i++);
                    chr2 = input.charCodeAt(i++);
                    chr3 = input.charCodeAt(i++);

                    enc1 = chr1 >> 2;
                    enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                    enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                    enc4 = chr3 & 63;

                    if (isNaN(chr2)) {
                        enc3 = enc4 = 64;
                    } else if (isNaN(chr3)) {
                        enc4 = 64;
                    }

                    output = output +
                        keyStr.charAt(enc1) +
                        keyStr.charAt(enc2) +
                        keyStr.charAt(enc3) +
                        keyStr.charAt(enc4);
                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";
                } while (i < input.length);

                return output;
            },

            decode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
                var base64test = /[^A-Za-z0-9\+\/\=]/g;
                if (base64test.exec(input)) {
                    window.alert("There were invalid base64 characters in the input text.\n" +
                        "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                        "Expect errors in decoding.");
                }
                input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

                do {
                    enc1 = keyStr.indexOf(input.charAt(i++));
                    enc2 = keyStr.indexOf(input.charAt(i++));
                    enc3 = keyStr.indexOf(input.charAt(i++));
                    enc4 = keyStr.indexOf(input.charAt(i++));

                    chr1 = (enc1 << 2) | (enc2 >> 4);
                    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                    chr3 = ((enc3 & 3) << 6) | enc4;

                    output = output + String.fromCharCode(chr1);

                    if (enc3 != 64) {
                        output = output + String.fromCharCode(chr2);
                    }
                    if (enc4 != 64) {
                        output = output + String.fromCharCode(chr3);
                    }

                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";

                } while (i < input.length);

                return output;
            }
        };

    });
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