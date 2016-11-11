
myApp.factory('searchService', function ($http, eventService, deinitializationService) {

    var service = {};

    service.searchEvents = [];

    service.searchCalendars = [];

    service.retrieveSearchEvents = function(token) {
        return $http({
            method: "POST",
            url: "api/Global/GetSearchEvents",
            params: { token: token },
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.retrieveSearchCalendars = function(token) {
        return $http({
            method: "POST",
            url: "api/Global/GetSearchCalendars",
            params: { token: token },
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.retrieveSearchEventsByCalendarId = function (token, calendarId) {
        return $http({
            method: "POST",
            url: "api/Global/GetSearchEventsByCalendarId",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });  
    }

    service.setSearchEvents = function(searchEvents) {
        service.searchEvents = searchEvents;
    }

    service.setSearchCalendars = function(searchCalendars) {
        service.searchCalendars = searchCalendars;
    }

    service.addSearchEvent = function(searchEvent) {
        service.searchEvents.push(searchEvent);
    }

    service.addSearchCalendar = function(searchCalendar) {
        service.searchCalendars.push(searchCalendar);
    }

    service.deleteSearchCalendar = function(calendarId) {
        for (var i = 0; i < service.searchCalendars.length; i++) {
            if (service.searchCalendars[i].Id === calendarId) {
                service.searchCalendars.splice(i, 1);
                i--;
                break;
            }
        }
    }

    service.deleteEvent = function(eventId) {
        for (var i = 0; i < service.searchEvents.length; i++) {
            if (service.searchEvents[i].Id === eventId) {
                service.searchEvents.splice(i, 1);
                i--;
                break;
            }
        }
    }

    service.modifyEvent = function(eventId) {
        var updateSearchEvent = false;
        var searchIndex;
        for (var i = 0; i < service.searchEvents.length; i++) {
            if (service.searchEvents[i].Id === eventId) {
                updateSearchEvent = true;
                searchIndex = i;
                break;
            }
        }
        if (updateSearchEvent === true) {
            var token = localStorage.getItem("token");
            eventService.retrieveEventById(token, eventId)
                .then(function (response) {
                    if (service.searchEvents[searchIndex].Title !== response.data.Title) {
                        service.searchEvents[searchIndex].Title = response.data.Title;
                    }
                }, function (response) {
                    if (response.status === 401) {
                        deinitializationService.appRefresh();
                    }
                });
        }
    }

    service.addUserPrivateEvents = function(events) {
        for (var key in events) {
            if (events.hasOwnProperty(key)) {
                if (events[key].isPublic === false) {
                    var ownerName = events[key].owner.FirstName + events[key].owner.LastName;
                    var searchEvent = {
                        Id: events[key].id,
                        Title: events[key].title,
                        OwnerName: ownerName
                    }
                    service.searchEvents.push(searchEvent);
                }
            }
        }
    }

    service.addUserPrivateCalendars = function(calendars) {
        for (var key in calendars) {
            if (calendars.hasOwnProperty(key)) {
                if (calendars[key].isPublic === false) {
                    var ownerName = calendars[key].owner.FirstName + calendars[key].owner.LastName;
                    var searchCalendar = {
                        Id: calendars[key].id,
                        Title: calendars[key].title,
                        OwnerName: ownerName
                    }
                    service.searchCalendars.push(searchCalendar);
                }
            }
        }
    }

    return service;
})