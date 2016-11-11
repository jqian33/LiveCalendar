
myApp.factory('hubService', function ($rootScope, timelineService, deinitializationService, searchService, userCalendarsService, subscribedEventsService, config, eventService, exploreCalendarsService, userService) {

    var service = {};

    service.initializeHub = function () {
        $.connection.hub.start().done(console.log("Hub Started!"));
    }

    $.connection.eventHub.client.broadcastNewEvent = function (searchEvent, calendarId) {
        var token = localStorage.getItem("token");
        searchService.addSearchEvent(searchEvent);
        if (userCalendarsService.calendars.hasOwnProperty(calendarId)) {
            eventService.retrieveEventById(token, searchEvent.Id)
                .then(function (response) {
                    var serverEvent = response.data;
                    userCalendarsService.addEvent(serverEvent, calendarId);
                }, function (response) {
                    if (response.status === 401) {
                        deinitializationService.appRefresh();
                    }
                });
        }
        else if (exploreCalendarsService.cachedCalendars.hasOwnProperty(calendarId)) {
            eventService.retrieveEventById(token, searchEvent.Id)
                .then(function (response) {
                    var serverEvent = response.data;
                    exploreCalendarsService.addEvent(serverEvent, calendarId);
                }, function (response) {
                    if (response.status === 401) {
                        deinitializationService.appRefresh();
                    }
                });
        }
        timelineService.retrieveTimelineEvent(token, searchEvent.Id)
            .then(function(response) {
                timelineService.addEntry(response.data);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
        $rootScope.$apply();
    };

    $.connection.eventHub.client.broadcastDeletedEvent = function (eventId, calendarId, eventStart) {
        searchService.deleteEvent(eventId);
        if (subscribedEventsService.events.hasOwnProperty(eventId)) {
            subscribedEventsService.removeEvent(eventId);
        } else {
            if (userCalendarsService.events.hasOwnProperty(eventId)) {
                userCalendarsService.removeEvent(eventId, calendarId);
            }
            else if (exploreCalendarsService.cachedCalendars.hasOwnProperty(calendarId)) {
                exploreCalendarsService.removeEvent(eventId, calendarId);
            }
        }
        timelineService.deleteEntry(eventId, eventStart);
        $rootScope.$apply();
    }

    $.connection.eventHub.client.broadcastEditedEvent = function (eventId) {
        var token = localStorage.getItem("token");
        if (userCalendarsService.events.hasOwnProperty(eventId)) {
            var event = userCalendarsService.events[eventId];
            if (event.owner.Id !== userService.user.Id) {
                eventService.retrieveEventById(token, eventId)
                    .then(function (response) {
                        userCalendarsService.updateEvent(response.data);
                        console.log(response.data);
                    }, function (response) {
                        if (response.status === 401) {
                            deinitializationService.appRefresh();
                        }
                    });
            }
        }
        else if (subscribedEventsService.events.hasOwnProperty(eventId)) {
            eventService.retrieveEventById(token, eventId)
                .then(function (response) {
                    subscribedEventsService.updateEvent(response.data);
                    console.log(response.data);
                }, function (response) {
                    if (response.status === 401) {
                        deinitializationService.appRefresh();
                    }
                });
        }
        else if (exploreCalendarsService.calendar !== null && exploreCalendarsService.events.hasOwnProperty(eventId)) {
            eventService.retrieveEventById(token, eventId)
                .then(function (response) {
                    exploreCalendarsService.updateEvent(response.data);
                    console.log(response.data);
                }, function (response) {
                    if (response.status === 401) {
                        deinitializationService.appRefresh();
                    }
                });
        }
        searchService.modifyEvent(eventId);
        $rootScope.$apply();
    }


    $.connection.eventHub.client.broadcastEditedEventStartDate = function (oldStartDate, eventId) {
        var token = localStorage.getItem("token");
        timelineService.deleteEntry(eventId, oldStartDate);
        timelineService.retrieveTimelineEvent(token, eventId)
            .then(function (response) {
                timelineService.addEntry(response.data);
            }, function (response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
        $rootScope.$apply();
    }

    $.connection.eventHub.client.broadcastAddedAttendee = function (eventId, user, eventStart) {
        if (userCalendarsService.events.hasOwnProperty(eventId)) {
            userCalendarsService.addAttendee(eventId, user);
        }
        else if (subscribedEventsService.events.hasOwnProperty(eventId)) {
            subscribedEventsService.addAttendee(eventId, user);
        }
        else if (exploreCalendarsService.events.hasOwnProperty(eventId)) {
            exploreCalendarsService.addAttendee(eventId, user);
        } else {
            if (eventService.selectedEvent !== null) {
                if (eventService.selectedEvent.id === eventId) {
                    eventService.selectedEvent.attendees.push(user);
                }
            }
        }
        timelineService.eventAddedAttendee(eventId, eventStart);
        $rootScope.$apply();
    }

    $.connection.eventHub.client.broadcastDeletedAttendee = function (eventId, userId, eventStart) {
        if (userCalendarsService.events.hasOwnProperty(eventId)) {
            userCalendarsService.removeAttendee(eventId, userId);
        }
        else if (subscribedEventsService.events.hasOwnProperty(eventId)) {
            subscribedEventsService.removeAttendee(eventId, userId);
        }
        else if (exploreCalendarsService.events.hasOwnProperty(eventId)) {
            exploreCalendarsService.removeAttendee(eventId, userId);
        } else {
            if (eventService.selectedEvent !== null) {
                if (eventService.selectedEvent.id === eventId) {
                    var attendees = eventService.selectedEvent.attendees;
                    for (var i = 0; i < attendees.length; i++) {
                        if (attendees[i].Id === userId) {
                            attendees.splice(i, 1);
                            break;
                        }
                    }
                }
            }
        }
        timelineService.eventRemovedAttendee(eventId, eventStart);
        $rootScope.$apply();
    }

    $.connection.calendarHub.client.broadcastNewCalendar = function (calendarId, title, ownerName, ownerId) {
        if (userService.user.Id !== ownerId) {
            var searchCalendar = {
                Id: calendarId,
                Title: title,
                OwnerName: ownerName
            }
            searchService.addSearchCalendar(searchCalendar);
            var token = localStorage.getItem("token");
            searchService.retrieveSearchEventsByCalendarId(token, calendarId)
                .then(function (response) {
                    var searchEvents = response.data;
                    for (var i = 0; i < searchEvents.length; i++) {
                        searchService.addSearchEvent(searchEvents[i]);
                    }
                    console.log(response.data);
                }, function (response) {
                    if (response.status === 401) {
                        deinitializationService.appRefresh();
                    }
                });
        }
        $rootScope.$apply();
    }

    $.connection.calendarHub.client.broadcastDeletedCalendar = function (calendarId, ownerId, eventIdList) {
        if (userService.user.Id !== ownerId) {
            if (userCalendarsService.calendars.hasOwnProperty(calendarId) === true) {
                userCalendarsService.removeCalendar(calendarId);
            }
            else if (exploreCalendarsService.calendar !== null) {
                if (exploreCalendarsService.calendar.id === calendarId) {
                    exploreCalendarsService.clearCalendar();
                    $rootScope.showExploreNotification = false;
                }
            }
            searchService.deleteSearchCalendar(calendarId);
            for (var i = 0; i < eventIdList.length; i++) {
                searchService.deleteEvent(eventIdList[i]);
            }
        }
        $rootScope.$apply();
    }

    return service;
})