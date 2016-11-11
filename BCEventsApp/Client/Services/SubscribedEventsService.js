
myApp.factory('subscribedEventsService', function($http, $rootScope, basicService, config) {
    var service = {};
   
    service.addedEventSource = null;
    service.removedEventSource = null;
    service.refreshedEventSource = null;

    service.events = {};
    service.displayEvents = {
        id: -1,
        title: "Subscribed Events",
        events: [],
        display: false,
        color: config.eventColors.subscribed
    };

    service.retrieveEvents = function(token) {
        return $http({
            method: "POST",
            url: "api/User/GetAllSubscribedEvents",
            params: { token: token },
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.setEvents = function(events) {
        for (var key in events) {
            if (events.hasOwnProperty(key)) {
                service.addServerEvent(events[key]);
            }
        }
    }

    service.addAttendee = function(eventId, user) {
        if (service.events.hasOwnProperty(eventId)) {
            var event = service.events[eventId];
            event.attendees.push(user);
            if (service.displayEvents.display === true) {
                service.refreshDisplayEvents();
            }
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
            if (service.displayEvents.display === true) {
                service.refreshDisplayEvents();
            }
        }
    }

    service.setDisplay = function(value) {
        service.displayEvents.display = value;
    }

    // Return raw server side event object, must be converted for UI consumption
    service.getEventById = function (eventId) {
        if (service.events.hasOwnProperty(eventId)) {
            return service.events[eventId];
        } else {
            return null;
        }
    }

    service.addServerEvent = function(eventObject) {
        if (eventObject.RRule != null) {
            var rule = new RRule(RRule.parseString(eventObject.RRule));
            var repeats = rule.all();
            for (var i = 0; i < repeats.length; i++) {
                var eventInstance = basicService.getRecurrenceInstance(repeats, eventObject, i);
                service.displayEvents.events.push(eventInstance);
            }
        } else {
            var event = basicService.serverToDisplayEvent(eventObject);
            service.displayEvents.events.push(event);
        }
        service.events[eventObject.Id] = basicService.serverToUiEvent(eventObject);
    }

    service.addEvent = function(event) {
        service.events[event.id] = event;
        var displayEvent;
        if (event.uiEvent === true) {
            displayEvent = basicService.uiToDisplayEvent(event);
        } else {
            if (event.exploreEvent === true) {
                displayEvent = basicService.exploreToSubscribedDisplayEvent(event);
            } else {
                displayEvent = event;
            }
        }
        if (displayEvent.rRule != null) {
            var rule = new RRule(RRule.parseString(displayEvent.rRule));
            var repeats = rule.all();
            for (var i = 0; i < repeats.length; i++) {
                var eventInstance = basicService.getRecurrenceInstanceFromDisplayEvent(repeats, displayEvent, i);
                service.displayEvents.events.push(eventInstance);
            }
        } else {
            service.displayEvents.events.push(displayEvent);
        }
        if (service.displayEvents.display === true) {
            service.refreshDisplayEvents();
        }
    }

    service.removeEvent = function(eventId) {
        for (var i = 0; i < service.displayEvents.events.length; i++) {
            if (service.displayEvents.events[i].id === eventId) {
                service.displayEvents.events.splice(i, 1);
                i--;
            }
        }
        if (service.events.hasOwnProperty(eventId)) {
            delete service.events[eventId];
        }
        if (service.displayEvents.display === true) {
            service.refreshDisplayEvents();
        }
    }

    service.updateEvent = function(event) {
        service.removeEvent(event.Id);
        service.addServerEvent(event);
    }

    service.toggle = function() {
        var events = service.displayEvents;
        if (events.display === false) {
            service.addedEventSource = events;
            service.setDisplay(true);
        } else {
            service.removedEventSource = events;
            service.setDisplay(false);
        }
    }

    service.calendarSubscribed = function(calendarId) {
        for (var key in service.events) {
            if (service.events.hasOwnProperty(key)) {
                if (service.events[key].calendarId === calendarId) {
                    delete service.events[key];
                }
            
            }
        }
        for (var i = 0; i < service.displayEvents.events.length; i++) {
            if (service.displayEvents.events[i].calendarId === calendarId) {
                service.displayEvents.events.splice(i, 1);
                i--;
            }
        }
        if (service.displayEvents.display === true) {
            service.refreshDisplayEvents();
        }
    }

    service.refreshDisplayEvents = function() {
        if (service.displayEvents.display === true) {
            service.refreshedEventSource = service.displayEvents;
        }
    }

    service.resetAddedEventSource = function() {
        service.addedEventSource = null;
    }

    service.resetRemovedEventSource = function () {
        service.removedEventSource = null;
    }

    service.resetRefreshedEventSource = function() {
        service.refreshedEventSource = null;
    }

    return service;
})