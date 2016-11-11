
myApp.factory('eventCreationService', function ($http) {

    var service = {};

    service.createEvent = function(token, eventObject) {
        return $http({
            method: "POST",
            url: "api/User/CreateEvent",
            params: {token: token},
            data: eventObject,
            headers: { 'Content-Type': 'application/json' }
        });
    };

    return service;

})