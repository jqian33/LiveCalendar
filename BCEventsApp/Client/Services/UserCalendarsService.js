
myApp.factory('userCalendarsService', function($rootScope, $http, basicService, config, userService) {
    var service = {};

    service.addedEventSource = null;
    service.removedEventSource = null;
    service.refreshedEventSource = null;

    // Collection of events wrapped as calendar objects ready to be rendered in UI
    service.calendars = {};

    service.events = {};
    service.serverCalendars = {};
    service.primaryCalendarId = null;
    service.userId = null;
    service.selectedDate = null;

    service.ownedCalendars = [];
    service.subscribedCalendars = [];

    service.setUserId = function(userId) {
        service.userId = userId;
    }

    service.setPrimaryCalendarId = function(calendarId) {
        service.primaryCalendarId = calendarId;
    }

    service.setCalendars = function (data, userId) {
        for (var i = 0; i < data.length; i++) {
            service.addCalendar(data[i], userId);
            service.serverCalendars[data[i].Id] = data[i];
        }
    };

    service.addCalendar = function(data, userId) {
        var eventObjects = [];
        var events = data.Events;
        for (var key in events) {
            if (events.hasOwnProperty(key)) {
                service.addServerEvent(events[key], eventObjects);
            }
        }
        var title;
        if (data.Title === "Primary Calendar" && data.Owner.Id !== userService.user.Id) {
            title = data.Title + " (" + data.Owner.Id + ")";
        } else {
            title = data.Title;
        }
        var calendar = {
            id: data.Id,
            title: title,
            description: data.Description,
            owner: data.Owner,
            events: eventObjects,
            tags: data.Tags,
            isPublic: data.Public,
            display: false
        };

        if (data.Owner.Id === userId) {
            calendar.color = config.eventColors.owned;
            service.ownedCalendars.push(calendar);
        } else {
            calendar.color = config.eventColors.subscribed;
            service.subscribedCalendars.push(calendar);
        }
        service.calendars[data.Id] = calendar;
    }

    service.addAttendee = function (eventId, user)
    {
        if (service.events.hasOwnProperty(eventId)) {
            var event = service.events[eventId];
            event.attendees.push(user);
            var calendar = service.calendars[event.calendarId];
            if (calendar.display === true) {
                service.refreshedEventSource = calendar;
            }
        }
    }

    service.removeAttendee = function(eventId, userId) {
        if (service.events.hasOwnProperty(eventId)) {
            var event = service.events[eventId];
            for (var i = 0; i < event.attendees.length; i++) {
                if (event.attendees[i].Id === userId) {
                    event.attendees.splice(i, 1);
                    break;
                }
            }
            var calendar = service.calendars[event.calendarId];
            if (calendar.display === true) {
                service.refreshedEventSource = calendar;
            }
        }
    } 

    service.addServerEvent = function(eventObject, container) {
        if (eventObject.RRule != null) {
            var rule = new RRule(RRule.parseString(eventObject.RRule));
            var repeats = rule.all();
            for (var j = 0; j < repeats.length; j++) {
                var eventInstance = basicService.getRecurrenceInstance(repeats, eventObject, j);
                container.push(eventInstance);
            }
        } else {
            var event = basicService.serverToDisplayEvent(eventObject);
            container.push(event);
        }
        service.events[eventObject.Id] = basicService.serverToUiEvent(eventObject);
    }

    service.addEvent = function (eventObject, calendarId) {
        service.addServerEvent(eventObject, service.calendars[calendarId].events);

        var display = service.getDisplay(calendarId);
        var calendar = service.getCalendar(calendarId);
        if (display === true) {
            service.refreshedEventSource = calendar;
        }
    };

    service.removeEvent = function(eventId, calendarId) {
        var events = service.calendars[calendarId].events;
        var calendar = service.getCalendar(calendarId);
        var display = service.getDisplay(calendarId);
        for (var i = 0; i < events.length; i++) {
            if (events[i].id === eventId) {
                events.splice(i, 1);
                i--;
            }
        }
        delete service.events[eventId];
        if (display === true) {
            service.refreshedEventSource = calendar;
        }
    }

    service.removeCalendar = function (calendarId) {
        var calendar = service.calendars[calendarId];
        for (var i = 0; i < calendar.events.length; i++) {
            var eventId = calendar.events[i].id;
            delete service.events[eventId];
        }
        service.removedEventSource = service.calendars[calendarId];
        delete service.calendars[calendarId];
        for (var j = 0; j < service.ownedCalendars.length; j++) {
            if (service.ownedCalendars[j].id === calendarId) {
                service.ownedCalendars.splice(j, 1);
                j--;
            }
        }
        for (var k = 0; k < service.subscribedCalendars.length; k++) {
            if (service.subscribedCalendars[k].id === calendarId) {
                service.subscribedCalendars.splice(k, 1);
                k--;
            }
        }
    }

    service.getDisplay = function(calendarId) {
        return service.calendars[calendarId].display;
    }

    service.setDisplay = function(calendarId, value) {
        service.calendars[calendarId].display = value;
    }

    service.getCalendar = function (calendarId) {
        if (service.calendars.hasOwnProperty(calendarId)) {
            return service.calendars[calendarId];
        } else {
            return null;
        }
    };

    service.setSelectedDate = function(date) {
        service.selectedDate = date;
    }

    service.retrieveCalendars = function(token) {
        return $http({
            method: "POST",
            url: "api/User/GetAllCalendars",
            params: { token: token },
            headers: { 'Content-Type': 'application/json' }
        });
    };

    service.retrievePrimaryCalendarId = function(token) {
        return $http({
            method: "POST",
            url: "api/User/GetPrimaryCalendarId",
            params: { token: token },
            headers: { 'Content-Type': 'application/json' }
        });
    };

    // Return raw server side event object, must be converted for UI consumption
    service.getEventById = function (eventId) {
        if (service.events.hasOwnProperty(eventId)) {
            return service.events[eventId];
        } else {
            return null;
        }
    }

    // Parameter is server event
    service.updateEvent = function (event) {
        service.removeEvent(event.Id, event.CalendarId);
        service.addEvent(event, event.CalendarId);
    }

    service.resetAddedEventSource = function() {
        service.addedEventSource = null;
    }

    service.resetRemovedEventSource = function() {
        service.removedEventSource = null;
    }

    service.resetRefreshedEventSource = function() {
        service.refreshedEventSource = null;
    }

    service.resetRemovedEventId = function() {
        service.removedEventId = null;
    }

    service.toggleCalendar = function(calendarId) {
        var calendar = service.getCalendar(calendarId);
        var display = service.getDisplay(calendarId);
        if (display === false) {
            service.addedEventSource = calendar;
            service.setDisplay(calendarId, true);
        } else {
            service.removedEventSource = calendar;
            service.setDisplay(calendarId, false);
        }
    }

    service.modifyCalendarPrivacy = function(calendarId, value) {
        if (service.calendars.hasOwnProperty(calendarId)) {
            var calendar = service.calendars[calendarId];
            calendar.isPublic = value;
            var events = calendar.events;
            for (var i = 0; i < events.length; i++) {
                events[i].isPublic = value;
                service.events[events[i].id].isPublic = value;
            }
        }
    }

    return service;

});
