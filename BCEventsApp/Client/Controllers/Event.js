
myApp.controller('eventController', function ($scope, $rootScope, deinitializationService, timelineService, calendarService, eventService, userService, userCalendarsService, subscribedEventsService, searchService, exploreCalendarsService, config) {

    $scope.showEditButton = false;
    $scope.showDeleteButton = false;
    $scope.showSubscribeButton = false;
    $scope.showUnsubscribeButton = false;

    $scope.initModal = function () {
        var timeFormat = "dddd, MMMM Do YYYY, h:mm a";
        console.log(eventService.selectedEvent);
        $scope.eventTitle = eventService.selectedEvent.title;
        $scope.eventDescription = eventService.selectedEvent.description;
        $scope.eventLocation = eventService.selectedEvent.location;
        $scope.eventStartTime = eventService.selectedEvent.start.format(timeFormat);
        $scope.eventEndTime = eventService.selectedEvent.end.format(timeFormat);
        $scope.renderSubscribeButton(eventService.selectedEvent.id);
        $scope.attendees = eventService.selectedEvent.attendees;
        var calendarId = eventService.selectedEvent.calendarId;
        if (userCalendarsService.calendars.hasOwnProperty(calendarId)) {
            $scope.calendarTitle = userCalendarsService.calendars[calendarId].owner.Id + "'s " + userCalendarsService.calendars[calendarId].title;
        } else {
            var token = localStorage.getItem("token");
            calendarService.retrieveCalendarInfo(token, calendarId)
                .then(function (response) {
                    if (response.data !== null) {
                        $scope.calendarTitle = response.data.UserId + "'s " + response.data.Title;
                    } else {
                        $scope.calendarTitle = null;
                    }
                }, function(response) {
                    if (response.status === 401) {
                        deinitializationService.appRefresh();
                    }
                });
        }
        $scope.numberOfAttendees = eventService.selectedEvent.attendees.length;
        $scope.attendees = eventService.selectedEvent.attendees;
        $scope.attendance = $scope.checkAttendance(eventService.selectedEvent.attendees);
    };

    $scope.checkAttendance = function(list) {
        var attendance = false;
        for (var i = 0; i < list.length; i++) {
            if (list[i].Id === userService.user.Id) {
                attendance = true;
                break;
            }
        }
        return attendance;
    }

    $scope.attend = function () {
        var token = localStorage.getItem("token");
        eventService.attendRequest(token, eventService.selectedEvent.id)
            .then(function() {
                var user = userService.user;
                $.connection.eventHub.server.addAttendee(eventService.selectedEvent.id, user, eventService.selectedEvent.start);
                $scope.attendance = true;
            }, function(response) {
                console.log(response);
            });
    }

    $scope.withdraw = function() {
        var token = localStorage.getItem("token");
        eventService.withdrawRequest(token, eventService.selectedEvent.id)
            .then(function() {
                var userId = userService.user.Id;
                $.connection.eventHub.server.removeAttendee(eventService.selectedEvent.id, userId, eventService.selectedEvent.start);
                $scope.attendance = false;
            }, function(response) {
                console.log(response);
            });
    }

    $scope.$watch(function() { return $scope.attendees }, function(newValue) {
        if (newValue !== undefined && newValue !== null) {
            $scope.numberOfAttendees = newValue.length;
            $scope.attendees = newValue;
        }
    }, true);

    $scope.deleteEvent = function() {
        var request = {
            CalendarId: eventService.selectedEvent.calendarId,
            EventId: eventService.selectedEvent.id
        }
        // need to be replaced with real token
        var token = localStorage.getItem("token");
        eventService.deleteRequest(token, request)
            .then(function() {
                userCalendarsService.removeEvent(request.EventId, request.CalendarId);
                if (eventService.selectedEvent.isPublic === true) {
                    $.connection.eventHub.server.deleteEvent(eventService.selectedEvent.id, eventService.selectedEvent.calendarId, eventService.selectedEvent.start);
                    timelineService.deleteEntry(eventService.selectedEvent.id, eventService.selectedEvent.start);
                } 
                searchService.deleteEvent(eventService.selectedEvent.id);
                $scope.modalDisplay = false;
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
        $rootScope.eventModalDisplay = false;
    }

    $scope.subscribeEvent = function() {
        var token = localStorage.getItem("token");
        eventService.subscribeRequest(token, eventService.selectedEvent.id)
            .then(function () {
                exploreCalendarsService.changeEventColor(eventService.selectedEvent.id, config.eventColors.subscribed);
                subscribedEventsService.addEvent(eventService.selectedEvent);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
        $rootScope.eventModalDisplay = false;
    }

    $scope.unsubscribeEvent = function() {
        var token = localStorage.getItem("token");
        eventService.unsubscribeRequest(token, eventService.selectedEvent.id)
            .then(function () {
                exploreCalendarsService.changeEventColor(eventService.selectedEvent.id, config.eventColors.explore);
                subscribedEventsService.removeEvent(eventService.selectedEvent.id);    
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
        $rootScope.eventModalDisplay = false;
    }

    // Check event is part of user's calendars or subscribed events
    $scope.renderSubscribeButton = function (eventId) {
        var userId = userService.user.Id;
        if (eventService.selectedEvent.owner.Id === userId) {
            $scope.showDeleteButton = true;
            $scope.showEditButton = true;
        }
        if (subscribedEventsService.getEventById(eventId) === null && userCalendarsService.getEventById(eventId) === null) {
            $scope.showSubscribeButton = true;
        }
        else if (subscribedEventsService.getEventById(eventId) !== null) {
            $scope.showUnsubscribeButton = true;
        }
    }

    $scope.showCalendar = function () {
        var calendarId = eventService.selectedEvent.calendarId;
        var cachedExploreCalendar = exploreCalendarsService.getCachedCalendar(calendarId);
        if (userCalendarsService.serverCalendars.hasOwnProperty(calendarId)) {
            calendarService.setSelectedCalendar(userCalendarsService.serverCalendars[calendarId]);
        }
        else if (cachedExploreCalendar !== null) {
            calendarService.setSelectedCalendar(cachedExploreCalendar);
        } else {
            var token = localStorage.getItem("token");
            calendarService.retrieveCalendarById(token, calendarId)
            .then(function (response) {
                calendarService.setSelectedCalendar(response.data);
            }, function (response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
        }
        $rootScope.calendarModalDisplay = true;
        $rootScope.eventModalDisplay = false;
    }

    $scope.showEventEdit = function () {
        $rootScope.eventModalDisplay = false;
        $rootScope.showEventEdit= true;
    }
})