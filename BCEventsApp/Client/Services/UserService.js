
myApp.factory('userService', function($http) {
    var service = {};
    service.user = {};

    service.retrieveUser = function(token) {
        return $http({
            method: "POST",
            url: "api/Global/GetUser",
            params: {token: token},
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.setUser = function(user) {
        service.user = user;
    }

    return service;
})