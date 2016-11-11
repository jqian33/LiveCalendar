
myApp.directive("password", function() {
    return {
        link: function(scope, element) {
            var inputField = element.find('input');
            inputField.focus(function() {
                scope.editing = true;
                scope.$apply();
            }).blur(function () {
                scope.editing = false;
                var password = inputField.val();
                if (password.length < 8) {
                    scope.passwordShortAlert = true;
                    if (scope.passwordTooShort === false) {
                        scope.errorCount += 1;
                        scope.passwordTooShort = true;
                    }
                    element.addClass("has-error");
                } else {
                    scope.passwordShortAlert = false;
                    if (scope.passwordTooShort === true) {
                        scope.errorCount -= 1;
                        scope.passwordTooShort = false;
                    }
                    element.removeClass("has-error");
                }

                if (password !== scope.passwordRepeat) {
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
                } else if (oldValue === true && newValue === false ) {
                    element.removeClass("has-error");
                    scope.passwordNotMatchAlert = false;
                }
            }, true);
        }
    }
})