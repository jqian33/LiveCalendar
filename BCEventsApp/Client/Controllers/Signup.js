
myApp.controller("signupController", function($scope, accountService) {
    $scope.passwordShortAlert = false;
    $scope.passwordNotMatchAlert = false;
    $scope.usernameExistAlert = false;
    $scope.successAlert = false;

    $scope.passwordTooShort = true;
    $scope.passwordMismatch = true;

    $scope.signup = function () {
        var username = $scope.username;
        accountService.userIdExistsRequest(username)
            .then(function(response) {
                if (response.data === true) {
                    $scope.successAlert = false;
                    $scope.usernameExistAlert = true;
                } else {
                    var userInfo = {
                        Id: $scope.username,
                        FirstName: $scope.firstname,
                        LastName: $scope.lastname
                    }
                    var request = {
                        User: userInfo,
                        Password: $scope.password
                    }
                    accountService.accountCreationRequest(request)
                        .then(function() {
                            $scope.successAlert = true;
                            $scope.usernameExistAlert = false;
                        }, function(response) {
                            console.log(response);
                        });
                    
                }
            }, function(response) {
                console.log(response);
            });
    }
})