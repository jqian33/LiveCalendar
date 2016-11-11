
myApp.directive('eventCreationModal', function() {
    return {
        link: function (scope, element) {

            element.find('#startTime').timepicker();
            element.find('#endTime').timepicker();

            element.on('shown.bs.modal', function () {
                scope.errorCount = 1;
                scope.initModal();
                element.find("#startTime").timepicker('setTime', scope.selectedDateTime);
                element.find('#endTime').timepicker('setTime', scope.endTime);
                scope.$apply();
            });

            element.on('hidden.bs.modal', function () {
                scope.errorCount = 1;
                scope.title = undefined;
                scope.description = "";
                scope.location = "";
                element.find("#startDate").val("");
                element.find("#startTime").val("");
                element.find("#endTime").val("");
                element.find("#titleDisplay").removeClass("has-error");
                element.find('#startDateDisplay').removeClass("has-error");
                element.find("#startTimeDisplay").removeClass("has-error");
                element.find("#endTimeDisplay").removeClass("has-error");
                scope.validationLookup.startDate = true;
                scope.validationLookup.startTime = true;
                scope.validationLookup.endTime = true;
                scope.showPublicCheckBox = false;
                scope.$apply();
            });

            scope.$watch(function () { return scope.modalDisplay }, function (newValue, oldValue) {
                if (oldValue === true && newValue === false) {
                    element.modal('hide');
                }
            }, true);
        }
    }
})