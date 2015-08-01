(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Settings", ["userResource",
                                 "toaster",
                                 "localStorageService",
                                 "$rootScope",
                                 "FileUploader",
                                 Settings]);

    function Settings(userResource, toaster, localStorageService, $rootScope, FileUploader) {
        var scope = this;
        $rootScope.checkUser();
        scope.email = "prinay.panday@entelect.co.za";
        scope.user = [];
        scope.Name = '';
        scope.Surname = '';
        scope.uploader = new FileUploader();
        scope.UpdateUser = UpdateUser;
        GetUser();

        function GetUser() {
            scope.user = localStorageService.get('user'); 
        }

        function UpdateUser() {
            var userModel = {
                Id: scope.user.Id,
                SlackId: scope.user.SlackId,
                EmailAddress: scope.user.EmailAddress,
                DisplayName: scope.user.DisplayName,
                AwardedFineCount: scope.user.AwardedFineCount,
                PendingFineCount: scope.user.PendingFineCount,
                Fines: null
            };
            var promise = userResource.save({
                    action: "UpdateUser"
                },
                userModel
            );
            promise.$promise.then(function (data) {
                    toaster.pop('success', 'Update Successful', 'User was updated successfully');
                    scope.user = data;
                },
                function () {
                    toaster.error('Error', 'Failed to update User');
                });
        }


    }
}());