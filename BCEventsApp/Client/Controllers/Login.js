
myApp.controller('loginController', function($scope, $location, authenticationService) {

    $scope.showAlert = false;

    $scope.login = function () {
        var userLogin = {
            UserId: $scope.username,
            Password: $scope.password
        }
        authenticationService.authenticationRequest(userLogin)
            .then(function(response) {
                if (response.data === true) {
                    $scope.showAlert = false;
                    authenticationService.issueTokenRequest(userLogin.UserId)
                        .then(function(response) {
                            localStorage.setItem("token", response.data);
                            authenticationService.authenticationSuccess($scope.username);
                        }, function(response) {
                            console.log(response);
                        });
                } else {
                    $scope.showAlert = true;
                }
            }, function(response) {
                // Log Error
                console.log(response);
            });
    }

    
})