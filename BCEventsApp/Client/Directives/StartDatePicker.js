
myApp.directive('startDatePicker', function () {

    return {
        link: function (scope, element) {

            scope.startDate = new Pikaday(
            {
                field: element.find('input')[0],
                firstDay: 1,
                minDate: new Date(2000, 0, 1),
                maxDate: new Date(2020, 12, 31),
                yearRange: [2000, 2020]
            });

            var inputField = element.find('input');
            inputField.focus(function () {
                element.removeClass("has-error");
                scope.editing = true;
                scope.$apply();
            }).blur(function () {
                scope.editing = false;
                if (inputField.val() === "") {
                    element.addClass("has-error");
                    if (scope.validationLookup.startDate === true) {
                        scope.errorCount = scope.errorCount + 1;
                        scope.validationLookup.startDate = false;
                    }
                } else {
                    if (scope.validationLookup.startDate === false) {
                        scope.errorCount = scope.errorCount - 1;
                        scope.validationLookup.startDate = true;
                    }
                    var selectedDate = scope.startDate.getMoment().toDate();
                    var year = selectedDate.getFullYear();
                    var month = selectedDate.getMonth();
                    var date = selectedDate.getDate();

                    var adjustEndDate;
                    if (scope.selectedDateTime.getDate() !== scope.endTime.getDate()) {
                        adjustEndDate = true;
                    } else {
                        adjustEndDate = false;
                    }
                    scope.selectedDateTime.setFullYear(year);
                    scope.selectedDateTime.setMonth(month);
                    scope.selectedDateTime.setDate(date);

                    scope.endTime.setFullYear(year);
                    scope.endTime.setMonth(month);
                    scope.endTime.setDate(date);

                    if (adjustEndDate === true) {
                        scope.endTime.setDate(scope.endTime.getDate() + 1);
                    }
                }
                scope.startDate.setMoment(scope.startDate.getMoment());
                scope.$apply();
            });
        }
    }
})