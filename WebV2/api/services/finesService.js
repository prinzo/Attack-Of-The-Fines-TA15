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