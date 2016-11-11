
myApp.controller('exploreNotificationController', function ($scope, $rootScope, deinitializationService, exploreCalendarsService, userCalendarsService, subscribedEventsService, userService, calendarService) {

    $scope.dismiss = function () {
        exploreCalendarsService.clearCalendar();
        $rootScope.showExploreNotification = false;
    }

    $scope.$watch(function () { return exploreCalendarsService.calendar }, function () {
        if (exploreCalendarsService.calendar != null) {
            $scope.calendarTitle = exploreCalendarsService.calendar.title;
        }  
    }, true);

    $scope.subscribe = function () {
        var token = localStorage.getItem("token");
        var calendarId = exploreCalendarsService.calendar.id;
        var userId = userService.user.Id;

        calendarService.subscribeRequest(token, calendarId)
            .then(function () {
                var calendar = exploreCalendarsService.getCachedCalendar(calendarId);
                userCalendarsService.addCalendar(calendar, userId);
                userCalendarsService.toggleCalendar(calendarId);
                subscribedEventsService.calendarSubscribed(calendarId);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });

        $scope.dismiss();
    }
})