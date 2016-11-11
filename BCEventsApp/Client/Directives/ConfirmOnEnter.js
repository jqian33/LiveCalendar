
myApp.directive("confirmOnEnter", function() {
    return {
        link: function(scope, element) {
            element.keyup(function (event) {
                if (event.keyCode === 13) {
                    if (scope.errorCount === 0) {
                        scope.login();
                    }
                }
            });
        }
    }
})