
myApp.directive("signupTab", function($rootScope) {
    return {
        link: function(scope, element) {
            scope.errorCount = 5;
        }
    }
})