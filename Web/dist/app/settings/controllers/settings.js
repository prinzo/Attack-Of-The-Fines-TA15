(function () {
    "use strict";
    angular
        .module("entelectFines")
        .controller("Settings", ["userResource",
                                 "toaster",
                                 "localStorageService",
                                 "$rootScope",
                                 "$scope",
                                 Settings]);

    function Settings(userResource, toaster, localStorageService, $rootScope, $scope) {
        var scope = this;
        $rootScope.checkUser();
        scope.email = "prinay.panday@entelect.co.za";
        scope.user = [];
        scope.Name = '';
        scope.Surname = '';
        scope.uploadButtonVisible = false;
        scope.uploadImage = uploadImage;
        $scope.Image = '';


        scope.UpdateUser = UpdateUser;
        GetUser();


        var handleFileSelect = function (evt) {

            var file = evt.currentTarget.files[0];
            var reader = new FileReader();

            reader.onload = function (evt) {
                var img = new Image();
                img.src = evt.target.result;

                $scope.$apply(function (scope) {
                    $scope.Image = evt.target.result;
                });
            };

            reader.readAsDataURL(file);
            scope.showUpload = true;
            scope.showImage = true;

        };

        angular.element(document.querySelector('#fileInput')).on('change', handleFileSelect);

        function uploadImage() {
            var userModel = {
                Id: scope.user.Id,
                Image: $scope.Image
            };
            var promise = userResource.save({
                    action: "UpdateUserImage"
                },
                userModel
            );
            promise.$promise.then(function (data) {
                    toaster.pop('success', 'Update Successful', 'User image updated successfully');
                    scope.showUpload = false;
                    scope.showImage = false;
                    scope.user.Image = data.Image;
                    localStorageService.clearAll();
                    localStorageService.set('user', scope.user);
                },
                function () {
                    toaster.error('Error', 'Failed to update User image');
                });
        }

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