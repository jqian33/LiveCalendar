
myApp.directive("passwordVerify", function() {
    return {
        link: function(scope, element) {
            var inputField = element.find('input');
            inputField.focus(function() {
                scope.editing = true;
                scope.$apply();
            }).blur(function () {
                scope.editing = false;
                var passwordRe = inputField.val();
                if (passwordRe !== scope.password) {
                    if (scope.passwordMismatch === false) {
                        scope.errorCount += 1;
                        scope.passwordMismatch = true;
                    }
                } else {
                    if (scope.passwordMismatch === true) {
                        scope.errorCount -= 1;
                        scope.passwordMismatch = false;
                    }
                }
                scope.$apply();
            });

            scope.$watch(function () { return scope.passwordMismatch }, function (newValue, oldValue) {
                if (oldValue === false && newValue === true) {
                    element.addClass("has-error");
                    scope.passwordNotMatchAlert = true;
                } else if (oldValue === true && newValue === false) {
                    element.removeClass("has-error");
                    scope.passwordNotMatchAlert = false;
                }
            }, true);
        }
    }
})