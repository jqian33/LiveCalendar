
myApp.directive("loginSignupModal", function (authenticationService) {
    return {
        link: function (scope, element) {

            scope.$watch(function () { return authenticationService.requireLogin }, function (newValue) {
                if (newValue === true) {
                    scope.modalDisplay = true;
                    element.modal({ backdrop: 'static', keyboard: false });
                }
            }, true);

            scope.$watch(function () { return authenticationService.authenticated }, function (newValue, oldValue) {
                if (newValue === true && oldValue === false) {
                    element.modal('hide');
                }
            }, true);

        }
    }
})