(function () {
    "use strict";
    angular
    .module("entelectFines")
    .controller("Fines", ['toaster', '$ngBootbox', Fines]);

    function Fines(toaster, $ngBootbox) {
        var vm = this;
        
        vm.AwardFine = AwardFine;
        
        function AwardFine() {
            toaster.pop('success', "Award Fine", "This will be a modal to award fines");
            
            $ngBootbox.alert("I'm A motherfucking modal in a motherfucking angular app made by the UI hater!!!!!!", function() {
              Example.show("Hello world callback");
            });
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
