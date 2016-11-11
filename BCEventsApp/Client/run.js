
myApp.run(function ($rootScope, initializationService, authenticationService) {
    initializationService.initializeView();
    var token = localStorage.getItem("token");
    if (token === null) {
        authenticationService.requireLogin();
    } else {
        authenticationService.tokenVerificationRequest(token)
            .then(function (response) {
                if (response.data !== null) {
                    var userId = response.data.UserId;
                    localStorage.setItem("token", response.data.Token);
                    authenticationService.authenticationSuccess(userId);
                } else {
                    authenticationService.requireLogin();
                }
            }, function(response) {
                console.log(response);
            });
    }
    
    $rootScope.$watch(function () { return authenticationService.authenticated }, function (newValue) {
        if (newValue === true) {
            console.log("Authenticated!");
            initializationService.initialize(authenticationService.userId);
        } 
    }, true);

})