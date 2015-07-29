(function () {
    "use strict";
    angular
    .module("entelectFines")
    .controller("FineAwards", ['toaster', 'finesResource', FineAwards]);

    function FineAwards(toaster, finesResource) {
        var vm = this;
                
        vm.AwardFine = function() {
                        
            var fine = new finesResource();
            
            fine.data = {
                            NewFineModel : {
                                IssuerId:'9f909fc1-405d-43ff-8b9d-381160892c61',
                                RecipientId:'e8f891fb-eff6-4787-bd02-636e5edd93b6',
                                Reason:'test'
                            }
                        }; 
              
            finesResource.save(fine, function() {
                toaster.pop('success', "Fine Awarded", "Fine awarded successfully");
            }); 

        }  
    }
}());
 