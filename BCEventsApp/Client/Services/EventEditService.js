
myApp.factory("eventEditService", function($http) {

    var service = {};

    service.editRequest = function(token, eventObject) {
        return $http({
            method: "POST",
            url: "api/User/ModifyEvent",
            params: { token: token },
            data: eventObject,
            headers: { 'Content-Type': 'application/json' }
        });
    }

    return service;
})