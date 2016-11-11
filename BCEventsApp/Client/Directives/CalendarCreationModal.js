
myApp.directive("calendarCreationModal", function($rootScope) {
    return {
        link: function(scope, element) {

            element.on('hidden.bs.modal', function () {
                $rootScope.showCalendarCreation = false;
                scope.title = undefined;
                scope.description = "";
                scope.isPublic = false;
                element.find("#calendarTitleDisplay").removeClass("has-error");
                scope.$apply();
                $rootScope.$apply();
            });

            element.on('shown.bs.modal', function() {
                element.find("#createCalendarButton").attr('disabled', 'disabled');
                scope.errorCount = 1;
                scope.$apply();
            });

            $rootScope.$watch(function () { return $rootScope.showCalendarCreation }, function (newValue, oldValue) {
                if (newValue === true) {
                    element.modal();
                }
                else if (newValue === false) {
                    element.modal('hide');
                }
            }, true);
        }
    }
})