(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Fines", ['toaster', '$ngBootbox',
                              'localStorageService',
                              '$rootScope',
                              Fines]);

    function Fines(toaster, $ngBootbox, localStorageService, $rootScope) {
        var vm = this;
        $rootScope.checkUser();
        vm.AwardFine = AwardFine;

        function AwardFine() {
            toaster.pop('success', "Award Fine", "This will be a modal to award fines");


        }

        function PayFine() {
            toaster.pop('success', "Pay Fine", "This will be a modal to pay fines");
        }

        function ViewUserStatistics() {
            toaster.pop('success', "User Statustics", "This will be a modal to view the selected user's statistics");
        }

        function SecondFine() {
            toaster.pop('success', "Seconded Fine", "This will second a fine");
        }

        function LikeFinePayment() {
            toaster.pop('success', "Like", "This will like the payment of a fine");
        }

        function DislikeFinePayment() {
            toaster.pop('success', "Dislike", "This will dislike the payment of a fine");
        }
    }
}());