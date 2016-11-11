
myApp.directive('startTimePicker', function() {
    return {
        link: function (scope, element) {
            var inputField = element.find('input');
            inputField.focus(function () {
                element.removeClass("has-error");
                scope.editing = true;
                scope.$apply();
            }).blur(function () {
                scope.editing = false;
                var startTime = inputField.timepicker('getTime');
                if (inputField.val() === "" || startTime == null) {
                    element.addClass("has-error");
                    if (scope.validationLookup.startTime === true) {
                        scope.errorCount = scope.errorCount + 1;
                        scope.validationLookup.startTime = false;
                    }
                } else {
                    if (scope.validationLookup.startTime === false) {
                        scope.errorCount = scope.errorCount - 1;
                        scope.validationLookup.startTime = true;
                    }
                    var h = startTime.getHours();
                    var m = startTime.getMinutes();
                    scope.selectedDateTime.setHours(h);
                    scope.selectedDateTime.setMinutes(m);
                }
                console.log(scope.selectedDateTime);
                scope.$apply();
            });
        }
    }
})