
myApp.directive('endTimePicker', function() {
    return {
        link: function (scope, element) {

            var calculateEndTime = function (startTime) {
                var endTime = new Date(startTime.getTime());
                endTime.setMinutes(endTime.getMinutes() + 60);
                return endTime;
            };

            var nextDayCheck = function () {
                if (scope.selectedDateTime >= scope.endTime) {
                    element.find('p').show();
                } else {
                    element.find('p').hide();
                }
            }

            var inputField = element.find('input');

            inputField.focus(function () {
                element.removeClass("has-error");
                scope.editing = true;
                scope.$apply();
            }).blur(function () {
                scope.editing = false;
                
                var endTime = inputField.timepicker('getTime');
                if (inputField.val() === "" || endTime == null) {
                    element.addClass("has-error");
                    if (scope.validationLookup.endTime === true) {
                        scope.errorCount = scope.errorCount + 1;
                        scope.validationLookup.endTime = false;
                    }
                } else {
                    if (scope.validationLookup.endTime === false) {
                        scope.errorCount = scope.errorCount - 1;
                        scope.validationLookup.endTime = true;
                    }
                    scope.endTime = endTime;
                    scope.endTime.setDate(scope.selectedDateTime.getDate());
                    scope.endTime.setMonth(scope.selectedDateTime.getMonth());
                    scope.endTime.setFullYear(scope.selectedDateTime.getFullYear());
                    nextDayCheck();
                }
                scope.$apply();
            });

            scope.$watch(function () { return scope.selectedDateTime }, function (newValue, oldValue) {
                if (newValue !== undefined && oldValue !== undefined) {
                    if (scope.endTimeStatic === true) {
                        scope.endTime = calculateEndTime(scope.selectedDateTime);
                        inputField.timepicker('setTime', scope.endTime);
                        element.removeClass("has-error");
                        if (scope.validationLookup.endTime === false) {
                            scope.errorCount = scope.errorCount - 1;
                            scope.validationLookup.endTime = true;
                        }
                    }
                    nextDayCheck();
                }
            }, true);

        }
    }
})