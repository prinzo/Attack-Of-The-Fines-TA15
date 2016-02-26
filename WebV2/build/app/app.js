var app = angular.module('entelectFines', ['entelectFines.login',
    'ui.router',
    'entelectFines.api',
    'entelectFines.fines',
    'ngCookies',
    'toaster',
    'angularSpinner',
    'angucomplete-alt',
    'ngAnimate',
    'ngAria',

    'ngMaterial'
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

            if ($state.current.name != 'login') {

                if (!$rootScope.globals.currentUser) {
                    event.preventDefault();

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
                .state('fines', {
                    url: '/fines/feed/:userId',
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
        angular
            .module('entelectFines.fines')
            .controller('FinesIndex', ['toaster',
                'FinesService',
                '$mdDialog',
                '$interval',
                '$rootScope',
                'appSettings',
                '$stateParams',
                FinesIndex]);


        function FinesIndex(toaster, FinesService, $mdDialog, $interval, $rootScope, appSettings, $stateParams) {
            $("#loadingFines").show();
            var scope = this;
            scope.settings = appSettings;
            scope.pagingIndex = 0;
            scope.dialogOptions = {
                $scope: scope
            }


            $rootScope.fines = {};

            UpdateFeed();

            $interval(UpdateFeed, 3000);

            function UpdateFeed() {
                var promise = FinesService.GetFines();

                promise.$promise.then(function (data) {
                        $("#loadingFines").hide();
                        $rootScope.fines = data;
                        scope.pagingIndex += data.length;
                    },

                    function () {
                        toaster.pop('error', "Fine Feed Failure", "No Fines were found");
                    });
            }


            scope.GetNextSetOfFines = function GetNextSetOfFines() {
                var promise = FinesService.GetNextSetOfFines();

                promise.$promise.then(function (data) {
                        $rootScope.fines.concat(data);
                        scope.pagingIndex += $rootScope.fines.length;
                    },

                    function () {
                        toaster.pop('error', "Fine Feed Failure", "No Fines were found");
                    });
            }

            scope.isOpen = false;

            scope.ShowAlertSecond = function (ev, Id) {
                var seconder = $("#seconder-" + Id).val();
                $mdDialog.show(
                    $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#fines-container')))
                        .clickOutsideToClose(true)
                        .title('Fine has been seconded')
                        .content('This fine has already been seconded by ' + seconder)
                        .ariaLabel('Fine has been seconded')
                        .ok('OK')
                        .targetEvent(ev)
                );
            }

            scope.Second = function Second(Id, issuerId, event) {
                var isSeconded = $("#isSeconded-" + Id).val();
                var currentUserId = $stateParams.userId;

                if (isSeconded == "true") {
                    scope.ShowAlertSecond(event, Id);
                } else if (currentUserId == issuerId) {
                    toaster.pop('error', "Fine Seconding Failure", "You cannot second a fine that you have awarded");
                } else {
                    var secondFineModel = {
                        UserId: currentUserId,
                        FineId: Id
                    };

                    var promise = FinesService.SecondFine(secondFineModel);

                    promise.$promise.then(function (data) {
                            toaster.pop('success', "Seconded", "Seconded");
                            $(".buttonSecond" + Id).removeClass('fa fa-angellist');
                            $(".buttonSecond" + Id).addClass('glyphicon glyphicon-ok');
                            $("#isSeconded-" + Id).val("true");
                            $("#seconder-" + Id).val('TEST');
                        },

                        function () {
                            toaster.pop('error', "Fine Feed Failure", "Could not second fine");
                        });
                }
            }

            scope.ShowAlertApproval = function ShowAlertApproval(ev, data, status) {

                var content = "<table class = 'table table-striped'>";

                data.forEach(function (entry) {
                    content += "<tr><td>" + entry.DisplayName + "</td></tr>";
                });

                content += "</table>";

                $mdDialog.show(
                    $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#fines-container')))
                        .clickOutsideToClose(true)
                        .title('Fine has been ' + status + ' by')
                        .content(content)
                        .ariaLabel('Fine has been ' + status + ' by')
                        .ok('OK')
                        .targetEvent(ev)
                );
            }

            scope.GetApprovedByUsers = function GetApprovedByUsers(Id, ApprovalStatus, event) {
                var service = ApprovalStatus == 1 ? "GetUserApprovedByList" : "GetUserDisapprovedByList";
                var status = ApprovalStatus == 1 ? "approved" : "disapproved";

                var fineModel = {
                    paymentId: Id
                };

                var promise = FinesService.GetApprovedOrDisapprovedByUsers(service, Id);

                promise.$promise.then(function (data) {
                        scope.ShowAlertApproval(event, data, status);
                    },

                    function () {
                        toaster.pop('error', "Fine Feed Failure", "Could not retrieve users");
                    });

            }

            scope.Approve = function Approve(Id) {

                var fineModel = {
                    UserId: $stateParams.userId,
                    FineId: Id
                };

                var promise = FinesService.ApprovePayment(fineModel);

                promise.$promise.then(function (data) {

                        if (data.Success == true) {
                            toaster.pop('success', "Approved", "Payment Approved");

                            if ($("#innerApproved-" + Id).length == 0) {
                                $("#approvedBy-" + Id).html('<div id="innerApproved-"' + Id + ' ng-if="fine.LikedByCount > 0">Approved by <a><span id="approvedByNumber-" + Id>1</span> person</a></div>');
                            } else {
                                var value = $("#approvedByNumber-" + Id).text();
                                $("#approvedByNumber-" + Id).text(parseInt(value) + 1);
                            }
                        } else if (data.Success == false) {
                            toaster.pop('success', "Approval Retracted", "Payment Approval was retracted");

                            var value = $("#approvedByNumber-" + Id).text();
                            if (value == '' || parseInt(value) - 1 == 0) {
                                $("#approvedByNumber-" + Id).text(parseInt(value) - 1)
                                $("#approvedBy-" + Id).html('');
                            } else {
                                $("#approvedByNumber-" + Id).text(parseInt(value) - 1);
                            }
                        }
                    },

                    function () {
                        toaster.pop('error', "Fine Feed Failure", "Payment could not be approved");
                    });
            }

            scope.Disapprove = function Disapprove(Id) {

                var fineModel = {
                    UserId: $stateParams.Id,
                    FineId: Id
                };

                var promise = FinesService.DisapprovePayment(fineModel);

                promise.$promise.then(function (data) {
                        if (data.Success == true) {
                            toaster.pop('success', "Disapproved", "Payment Disapproved");

                            if ($("#innerDisapproved-" + Id).length == 0) {
                                $("#disapprovedBy-" + Id).html('<div id="innerApproved-" + Id ng-if="fine.LikedByCount > 0">Disapproved by <a><span id="disapprovedByNumber-" + Id>1</span> person</a></div>');
                            } else {
                                var value = $("#disapprovedByNumber-" + Id).text();
                                $("#disapprovedByNumber-" + Id).text(parseInt(value) + 1);
                            }
                        } else if (data.Success == false) {
                            toaster.pop('success', "Disapproval Retracted", "Payment Disapproval was retracted");

                            var value = $("#disapprovedByNumber-" + Id).text();
                            if (value == '' || parseInt(value) - 1 == 0) {
                                $("#disapprovedByNumber-" + Id).text(parseInt(value) - 1);
                                $("#disapprovedBy-" + Id).html('');
                            } else {
                                $("#disapprovedByNumber-" + Id).text(parseInt(value) - 1);
                            }
                        }
                    },

                    function () {
                        toaster.pop('error', "Fine Feed Failure", "Payment could not be disapproved");
                    });
            }

            function DialogController($scope, $mdDialog, userId) {

                $rootScope.userId = userId;

                $scope.hide = function () {
                    $mdDialog.hide();
                };
                $scope.cancel = function () {
                    $mdDialog.cancel();
                };
                $scope.answer = function (answer) {
                    $mdDialog.hide(answer);
                };

            }

            scope.ShowStatistics = function (ev, id) {
                $mdDialog.show({
                    locals: {
                        userId: id
                    },
                    controller: DialogController,
                    templateUrl: 'app/fines/views/modals/userStatistics.tpl.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    clickOutsideToClose: true
                });
            };

            scope.ShowAddFine = function (ev) {
                $mdDialog.show({
                    locals: {
                        userId: null
                    },
                    controller: DialogController,
                    templateUrl: 'app/fines/views/modals/awardFine.tpl.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    clickOutsideToClose: true
                });
            };

            scope.ShowAddPayment = function (ev) {
                $mdDialog.show({
                    locals: {
                        userId: null
                    },
                    controller: DialogController,
                    templateUrl: 'app/fines/views/modals/payFine.tpl.html',
                    parent: angular.element(document.body),
                    targetEvent: ev,
                    clickOutsideToClose: true
                });
            };

            scope.ShowPaymentImage = function (ev, Image) {
                $mdDialog.show(
                    $mdDialog.alert()
                        .parent(angular.element(document.querySelector('#fines-container')))
                        .clickOutsideToClose(true)
                        .title('Payment Image')
                        .content('<img src="' + Image + '" class="image large-photo">')
                        .ariaLabel('Fine has been seconded')
                        .ok('OK')
                        .targetEvent(ev)
                );
            };
        }
    }

    ()
);
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
(function () {
    "use strict";

    angular
        .module("entelectFines.api", ["ngResource"])
        .constant("appSettings", {
            serverPath: "http://entelect.finesapi.local"
        });

}());
(function () {
    'use strict';

    angular
        .module('entelectFines.api')
        .factory('finesResource',
            [
                '$resource',
                'appSettings',
                finesResource
            ]);

    function finesResource($resource, appSettings) {
        return $resource(appSettings.serverPath + '/api/fines/:action/:id');

    }
}());
(function () {
    'use strict';

    angular
        .module('entelectFines.api')
        .factory('userResource',
            [
                '$resource',
                'appSettings',
                userResource
            ]);

    function userResource($resource, appSettings) {
        return $resource(appSettings.serverPath + '/api/user/:action/:id');

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

    .factory('FinesService', ['finesResource',
        function (finesResource) {
            var service = {};

            service.GetFines = GetFines;
            service.GetNextSetOfFines = GetNextSetOfFines;
            service.SecondFine = SecondFine;
            service.GetApprovedOrDisapprovedByUsers = GetApprovedOrDisapprovedByUsers;
            service.ApprovePayment = ApprovePayment;
            service.DisapprovePayment = DisapprovePayment;

            function GetFines() {
                var promise = finesResource.query({
                    action: "GetFines"
                });

                return promise;
            }

            function GetNextSetOfFines(index) {
                var promise = finesResource.query({
                    action: "GetNexSetOfFines",
                    index: index
                });

                return promise;
            }

            function SecondFine(secondFineModel) {
                var promise = finesResource.save({
                        action: "SecondFine"
                    },
                    secondFineModel
                );

                return promise;
            }

            function GetApprovedOrDisapprovedByUsers(action, Id) {
                var promise = finesResource.query({
                    action: action,
                    paymentId: Id
                });

                return promise;
            }

            function ApprovePayment(fineModel) {
                var promise = finesResource.save({
                        action: "ApprovePayment"
                    },
                    fineModel
                );

                return promise;
            }

            function DisapprovePayment(fineModel) {
                var promise = finesResource.save({
                        action: "DisapprovePayment"
                    },
                    fineModel
                );

                return promise;
            }

            return service;
        }])
'use strict';

angular.module('entelectFines.api')

    .factory('SpinnerService', ['usSpinnerService',
        function (usSpinnerService) {
            var service = {};
            service.StartSpin = StartSpin;
            service.StopSpin = StopSpin;

            function StartSpin() {
                usSpinnerService.spin('spinner-1');
            }

            function StopSpin() {
                usSpinnerService.stop('spinner-1');
            }

            return service;
        }])
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