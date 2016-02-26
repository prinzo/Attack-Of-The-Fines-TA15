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