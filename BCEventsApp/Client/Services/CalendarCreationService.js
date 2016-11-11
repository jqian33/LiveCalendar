
myApp.factory("calendarCreationService", function($http) {

    var service = {};

    service.createCalendar = function(token, calendarObject) {
        return $http({
            method: "POST",
            url: "api/User/CreateCalendar",
            params: { token: token },
            data: calendarObject,
            headers: { 'Content-Type': 'application/json' }
        });
    }

    return service;
})