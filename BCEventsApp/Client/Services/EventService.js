
myApp.factory('eventService', function($http, basicService) {
    var service = {};

    service.selectedEvent = null;

    service.deleteRequest = function(token, request) {
        return $http({
            method: "POST",
            url: "api/User/DeleteEvent",
            params: { token: token },
            data: request,
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.setSelectedEvent = function(event) {
        service.selectedEvent = event;
    }

    service.retrieveEventById = function(token, eventId) {
        return $http({
            method: "POST",
            url: "api/Global/GetEvent",
            params: { token: token },
            data: JSON.stringify(eventId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.setServerEvent = function(serverEvent) {
        service.selectedEvent = basicService.serverToUiEvent(serverEvent);
    }

    service.subscribeRequest = function(token, eventId) {
        return $http({
            method: "POST",
            url: "api/User/SubscribeEvent",
            params: { token: token },
            data: JSON.stringify(eventId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.unsubscribeRequest = function (token, eventId) {
        return $http({
            method: "POST",
            url: "api/User/UnsubscribeEvent",
            params: { token: token },
            data: JSON.stringify(eventId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.attendRequest = function(token, eventId) {
        return $http({
            method: "POST",
            url: "api/Event/AddAttendee",
            params: { token: token },
            data: JSON.stringify(eventId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.withdrawRequest = function(token, eventId) {
        return $http({
            method: "POST",
            url: "api/Event/RemoveAttendee",
            params: { token: token },
            data: JSON.stringify(eventId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    return service;
})