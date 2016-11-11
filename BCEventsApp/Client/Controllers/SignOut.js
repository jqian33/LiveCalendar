
myApp.controller("signOutController", function ($scope, deinitializationService) {
    
    $scope.signOut = function () {
        deinitializationService.appRefresh();
    }

})