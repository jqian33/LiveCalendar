
myApp.controller('userCalendarViewController', function ($rootScope, $scope, userCalendarsService, subscribedEventsService, exploreCalendarsService) {

    $scope.toggleCalendar = function (calendarId) {

        if (calendarId === -1) {
            subscribedEventsService.toggle();
        } else {
            userCalendarsService.toggleCalendar(calendarId);
        }
    }

    $scope.$watch(function () { return subscribedEventsService.displayEvents.display }, function (newValue, oldValue) {
        if (oldValue === true && newValue === false) {
            exploreCalendarsService.refresh();
        }
    }, true);

});