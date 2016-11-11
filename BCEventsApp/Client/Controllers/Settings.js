
myApp.controller('settingsController', function ($scope, $rootScope, deinitializationService, userService, userCalendarsService, calendarService) {

    $scope.unsubscribeCalendar = function(calendarId) {
        var token = localStorage.getItem("token");
        calendarService.unsubscribeRequest(token, calendarId)
            .then(function() {
                userCalendarsService.removeCalendar(calendarId);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
    }

    $scope.switched = function(calendarId, isPublic) {
        if (isPublic === true) {
            $scope.publishCalendar(calendarId);
        } else {
            $scope.unpublishCalendar(calendarId);
        }
    }

    $scope.publishCalendar = function (calendarId) {
        var token = localStorage.getItem("token");

        calendarService.publishRequest(token, calendarId)
            .then(function () {
                userCalendarsService.modifyCalendarPrivacy(calendarId, true);
                var title = userCalendarsService.calendars[calendarId].title;
                var ownerName = userService.user.FirstName + " " + userService.user.LastName;
                var ownerId = userService.user.Id;
                $.connection.calendarHub.server.addCalendar(calendarId, title, ownerName, ownerId);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
    }

    $scope.unpublishCalendar = function (calendarId) {
        var token = localStorage.getItem("token");

        calendarService.unpublishRequest(token, calendarId)
            .then(function () {
                var eventIdList = [];
                var events = userCalendarsService.calendars[calendarId].events;
                for (var i = 0; i < events.length; i++) {
                    eventIdList.push(events[i].id);
                }
                $.connection.calendarHub.server.deleteCalendar(calendarId, userService.user.Id, eventIdList);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
    }

    $scope.deleteCalendar = function (calendarId) {
        var token = localStorage.getItem("token");

        calendarService.deleteRequest(token, calendarId)
            .then(function () {
                var eventIdList = [];
                var events = userCalendarsService.calendars[calendarId].events;
                for (var i = 0; i < events.length; i++) {
                    eventIdList.push(events[i].id);
                }
                $.connection.calendarHub.server.deleteCalendar(calendarId, userService.user.Id, eventIdList);
                userCalendarsService.removeCalendar(calendarId);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
    }

})