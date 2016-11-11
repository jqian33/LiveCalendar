
myApp.factory('calendarService', function($http) {

    var service = {};

    // Calendar ready to render for UI
    service.selectedCalendar = null;

    // Calendar in server side format
    service.serverCalendar = null;

    service.setSelectedCalendar = function(calendar) {
        service.selectedCalendar = calendar;
    }

    service.retrieveCalendarById = function (token, calendarId) {
        return $http({
            method: "POST",
            url: "api/Global/GetCalendar",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.retrieveCalendarInfo = function(token, calendarId) {
        return $http({
            method: "POST",
            url: "api/Global/GetCalendarInfo",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.subscribeRequest = function(token, calendarId) {
        return $http({
            method: "POST",
            url: "api/User/SubscribeCalendar",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.unsubscribeRequest = function(token, calendarId) {
        return $http({
            method: "POST",
            url: "api/User/UnSubscribeCalendar",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.publishRequest = function(token, calendarId) {
        return $http({
            method: "POST",
            url: "api/User/PublishCalendar",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.unpublishRequest = function(token, calendarId) {
        return $http({
            method: "POST",
            url: "api/User/UnpublishCalendar",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.deleteRequest = function(token, calendarId) {
        return $http({
            method: "POST",
            url: "api/User/DeleteCalendar",
            params: { token: token },
            data: JSON.stringify(calendarId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    return service;
})