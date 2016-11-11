
myApp.factory("accountService", function($http) {

    var service = {};

    service.userIdExistsRequest = function (userId) {
        return $http({
            method: "POST",
            url: "api/Account/UserIdExists",
            data: JSON.stringify(userId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.accountCreationRequest = function(request) {
        return $http({
            method: "POST",
            url: "api/Account/CreateAccount",
            data: request,
            headers: { 'Content-Type': 'application/json' }
        });
    }

    return service;
})