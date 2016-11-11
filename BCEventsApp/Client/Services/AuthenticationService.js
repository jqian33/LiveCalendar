
myApp.factory('authenticationService', function($http) {

    var service = {}

    service.authenticated = false;

    service.requireLogin = false;

    service.userId = "";
    
    service.requireLogin = function() {
        service.requireLogin = true;
    }

    service.authenticationSuccess = function (userId) {
        service.userId = userId;
        service.authenticated = true;
    }

    service.authenticationRequest = function(userLogin) {
        return $http({
            method: "POST",
            url: "api/Authentication/Authenticate",
            data: userLogin,
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.tokenVerificationRequest = function(token) {
        return $http({
            method: "POST",
            url: "api/Authentication/VerifyToken",
            data: JSON.stringify(token),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.issueTokenRequest = function(userId) {
        return $http({
            method: "POST",
            url: "api/Authentication/IssueToken",
            data: JSON.stringify(userId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    return service;
})