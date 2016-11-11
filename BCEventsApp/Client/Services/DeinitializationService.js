
myApp.factory("deinitializationService", function($window) {

    var service = {};

    service.appRefresh = function () {
        localStorage.setItem("token", null);
        $.connection.hub.stop();
        $window.location.reload();
    }

    return service;
})