
myApp.factory('exploreCalendarsService', function(basicService, subscribedEventsService, config) {
    var service = {};

    service.calendar = null;
    service.events = {};
    service.addedEventSource = null;
    service.removedEventSource = null;
    service.refreshedEventSource = null;

    // Cache of server side calendars 
    service.cachedCalendars = {};

    service.setCalendar = function(data) {
        var eventObjects = [];
        var events = data.Events;
        for (var key in events) {
            if (events.hasOwnProperty(key)) {
                service.addServerEvent(events[key], eventObjects);
            }
        }

        var calendar = {
            id: data.Id,
            title: data.Title,
            description: data.Description,
            owner: data.Owner,
            events: eventObjects,
            tags: data.Tags,
            isPublic: data.Public
        };

        service.calendar = calendar;
        service.cachedCalendars[calendar.id] = data;
    }

    service.addServerEvent = function(eventObject, container) {
        if (eventObject.RRule != null) {
            var rule = new RRule(RRule.parseString(eventObject.RRule));
            var repeats = rule.all();
            for (var j = 0; j < repeats.length; j++) {
                var eventInstance = basicService.getRecurrenceInstance(repeats, eventObject, j);
                if (subscribedEventsService.events.hasOwnProperty(eventInstance.id)) {
                    eventInstance.color = config.eventColors.subscribed;
                } else {
                    eventInstance.color = config.eventColors.explore;
                }
                eventInstance.exploreEvent = true;
                container.push(eventInstance);
            }
        } else {
            var event = basicService.serverToDisplayEvent(eventObject);
            if (subscribedEventsService.events.hasOwnProperty(event.id)) {
                event.color = config.eventColors.subscribed;
            } else {
                event.color = config.eventColors.explore;
            }
            event.exploreEvent = true;
            container.push(event);
        }
        service.events[eventObject.Id] = basicService.serverToUiEvent(eventObject);
    }

    service.addEvent = function (eventObject, calendarId) {
        if (service.cachedCalendars.hasOwnProperty(calendarId)) {
            var eventId = eventObject.Id;
            service.cachedCalendars[calendarId].Events[eventId] = eventObject;
        }
        if (service.calendar !== null) {
            if (service.calendar.id === calendarId) {
                service.addServerEvent(eventObject, service.calendar.events);
                service.refresh();
            }
        }
    };

    service.addAttendee = function (eventId, user) {
        if (service.events.hasOwnProperty(eventId)) {
            var event = service.events[eventId];
            event.attendees.push(user);
            if (service.cachedCalendars.hasOwnProperty(event.calendarId)) {
                service.cachedCalendars[event.calendarId].Events[eventId].Attendees.push(user);
            }
            service.refresh();
        }
    }

    service.removeAttendee = function (eventId, userId) {
        if (service.events.hasOwnProperty(eventId)) {
            var event = service.events[eventId];
            for (var i = 0; i < event.attendees.length; i++) {
                if (event.attendees[i].Id === userId) {
                    event.attendees.splice(i, 1);
                    break;
                }
            }
            if (service.cachedCalendars.hasOwnProperty(event.calendarId)) {
                var attendees = service.cachedCalendars[event.calendarId].Events[eventId].Attendees;
                for (var j = 0; j < attendees.length; j++) {
                    if (attendees[j].Id === userId) {
                        attendees.splice(j, 1);
                    }
                }
            }
            service.refresh();
        }
    }

    service.removeEvent = function(eventId, calendarId) {
        if (service.cachedCalendars.hasOwnProperty(calendarId)) {
            delete service.cachedCalendars[calendarId].Events[eventId];
        }
        if (service.calendar !== null) {
            if (service.calendar.id === calendarId) {
                for (var i = 0; i < service.calendar.events.length; i++) {
                    if (service.calendar.events[i].id === eventId) {
                        service.calendar.events.splice(i, 1);
                        i--;
                    }
                }
                delete service.events[eventId];
                service.refresh();
            }
        }
    }

    service.updateEvent = function (eventObject) {
        service.removeEvent(eventObject.Id, eventObject.CalendarId);
        service.addEvent(eventObject, eventObject.CalendarId);
    }

    service.clearCalendar = function () {
        service.removedEventSource = service.calendar;
        service.events = {};
        service.calendar = null;
    }

    service.getCachedCalendar = function(calendarId) {
        if (service.cachedCalendars.hasOwnProperty(calendarId)) {
            return service.cachedCalendars[calendarId];
        } else {
            return null;
        }
    }

    service.show = function() {
        service.addedEventSource = service.calendar;
    }

    service.refresh = function() {
        if (service.calendar != null) {
            service.refreshedEventSource = service.calendar;
        }
    }

    service.changeEventColor = function(eventId, color) {
        if (service.calendar != null) {
            var events = service.calendar.events;
            for (var i = 0; i < events.length; i++) {
                if (events[i].id === eventId) {
                    events[i].color = color;
                }
            }
        }
        service.refresh();
    }

    service.resetAddedEventSource = function () {
        service.addedEventSource = null;
    }

    service.resetRemovedEventSource = function () {
        service.removedEventSource = null;
    }

    service.resetRefreshedEventSource = function () {
        service.refreshedEventSource = null;
    }

    return service;
})