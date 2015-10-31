(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Fines", ['toaster',
                              'localStorageService',
                              'finesResource',
                              '$rootScope',
                                '$mdDialog',
                              Fines]);

    function Fines(toaster, localStorageService, finesResource, $rootScope,$mdDialog) {
        var vm = this;

        vm.dialogOptions = {
                            $scope : vm
                       }
         
        $rootScope.checkUser();
        
        $rootScope.fines = {};
        
        var promise = finesResource.query({
                    action: "GetFines"
                }
            );
        
        promise.$promise.then(function (data) {
            $rootScope.fines = data;
            console.log(data);
        },
                              
        function () {
            toaster.pop('error', "Fine Feed Failure", "No Fines were found");
        });

        vm.isOpen = false;

        vm.ShowAlertSecond = function(ev, Id) {
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

        vm.Second = function Second(Id, event) {
            var isSeconded = $("#isSeconded").val();

            if(isSeconded) {
                vm.ShowAlertSecond(event, Id);
            } else {
                var secondFineModel = {
                    UserId: localStorageService.get('user').Id,
                    FineId: Id
                };

                var promise = finesResource.save({
                        action: "SecondFine"
                    },
                    secondFineModel
                );

                promise.$promise.then(function (data) {
                        toaster.pop('success', "Seconded", "Seconded");
                        $(".buttonSecond" + Id).removeClass('fa fa-angellist');
                        $(".buttonSecond" + Id).addClass('glyphicon glyphicon-ok');
                    },

                    function () {
                        toaster.pop('error', "Fine Feed Failure", "Could not second fine");
                    });
            }
        }

        vm.ShowAlertApproval = function ShowAlertApproval(ev, data, status) {

            var content = "<table class = 'table table-striped'>";

            data.forEach(function(entry) {
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

        vm.GetApprovedByUsers = function GetApprovedByUsers(Id, ApprovalStatus, event) {
            var service = ApprovalStatus == 1 ? "GetUserApprovedByList" : "GetUserDisapprovedByList";
            var status = ApprovalStatus == 1 ? "approved" : "disapproved";

            var fineModel = {
                paymentId: Id
            };

            var promise = finesResource.query({
                    action: service,
                    paymentId: Id
            });

            promise.$promise.then(function (data) {
                    vm.ShowAlertApproval(event, data, status);
                },

                function () {
                    toaster.pop('error', "Fine Feed Failure", "Could not retrieve users");
                });

        }

        vm.Approve = function Approve(Id) {

            var fineModel = {
                UserId: localStorageService.get('user').Id,
                FineId: Id
            };

            var promise = finesResource.save({
                    action: "ApprovePayment"
                },
                fineModel
            );

            promise.$promise.then(function (data) {

                    if(data.Success == true) {
                        toaster.pop('success', "Approved", "Payment Approved");

                        if ($("#innerApproved-" + Id).length == 0) {
                            $("#approvedBy-" + Id).html('<div id="innerApproved-"' + Id + ' ng-if="fine.LikedByCount > 0">Approved by <a><span id="approvedByNumber-" + Id>1</span> people</a></div>');
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

        vm.Disapprove = function Disapprove(Id) {

            var fineModel = {
                UserId: localStorageService.get('user').Id,
                FineId: Id
            };

            var promise = finesResource.save({
                    action: "DisapprovePayment"
                },
                fineModel
            );

            promise.$promise.then(function (data) {
                if(data.Success == true) {
                    toaster.pop('success', "Disapproved", "Payment Disapproved");

                    if ($("#innerDisapproved-" + Id).length == 0) {
                        $("#disapprovedBy-" + Id).html('<div id="innerApproved-" + Id ng-if="fine.LikedByCount > 0">Disapproved by <a><span id="disapprovedByNumber-" + Id>1</span> people</a></div>');
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

            $scope.hide = function() {
                $mdDialog.hide();
            };
            $scope.cancel = function() {
                $mdDialog.cancel();
            };
            $scope.answer = function(answer) {
                $mdDialog.hide(answer);
            };

        }

        vm.ShowAdvanced = function(ev, id) {
            $mdDialog.show({
                locals:{userId : id},
                controller: DialogController,
                templateUrl: 'app/fines/views/modals/userStatistics.tpl.html',
                parent: angular.element(document.body),
                targetEvent: ev,
                clickOutsideToClose:true
            });
        };

    }
}());