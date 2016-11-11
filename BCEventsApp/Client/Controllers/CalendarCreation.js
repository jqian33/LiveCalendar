
myApp.controller('calendarCreationController', function ($scope, $rootScope, initializationService, deinitializationService, calendarCreationService, userCalendarsService, userService, searchService) {
    
    $scope.createCalendar = function () {
        var title = $scope.title;
        var description = $scope.description;
        var isPublic = $scope.isPublic;
        var userId = userService.user.Id;

        var serverCalendar = {
            Id: "",
            Title: title,
            Description: description,
            Owner: null,
            Events: {},
            Tags: [],
            Public: isPublic
        }

        var token = localStorage.getItem("token");

        calendarCreationService.createCalendar(token, serverCalendar)
            .then(function(response) {
                serverCalendar.Id = response.data;
                serverCalendar.Owner = userService.user;
                userCalendarsService.addCalendar(serverCalendar, userId);
                var ownerName = serverCalendar.Owner.FirstName + " " + serverCalendar.Owner.LastName;
                if (serverCalendar.Public === true) {
                    $.connection.calendarHub.server.addCalendar(serverCalendar.Id, serverCalendar.Title, ownerName, serverCalendar.Owner.Id);
                }
                var searchCalendar = {
                    Id: serverCalendar.Id,
                    Title: serverCalendar.Title,
                    OwnerName: ownerName
                }
                searchService.addSearchCalendar(searchCalendar);
                $rootScope.showCalendarCreation = false;
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
    }

})