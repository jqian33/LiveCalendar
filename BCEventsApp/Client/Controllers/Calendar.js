
myApp.controller('calendarController', function ($scope, $rootScope, userService, calendarService, userCalendarsService, exploreCalendarsService) {

    $scope.showExploreButton = false;

    $scope.initModal = function () {
        var selected = calendarService.selectedCalendar;
        $scope.calendarTitle = calendarService.selectedCalendar.Title;
        $scope.calendarDescription = calendarService.selectedCalendar.Description;
        var ownerName = calendarService.selectedCalendar.Owner.FirstName + " " + calendarService.selectedCalendar.Owner.LastName;
        $scope.calendarOwner = ownerName;
        var eventCount = 0;
        for (var key in calendarService.selectedCalendar.Events) {
            if (calendarService.selectedCalendar.Events.hasOwnProperty(key)) {
                eventCount ++;
            }
        }
        $scope.eventCount = eventCount;
        $scope.renderButton(calendarService.selectedCalendar.Id);
        console.log(calendarService.selectedCalendar);
    };

    $scope.renderButton = function(calendarId) {
        if (userCalendarsService.calendars.hasOwnProperty(calendarService.selectedCalendar.Id) === false) {
            if (exploreCalendarsService.calendar == null) {
                $scope.showExploreButton = true;
            } else {
                if (exploreCalendarsService.calendar.id !== calendarId) {
                    $scope.showExploreButton = true;
                }
            }
        }
    }

    $scope.explore = function () {
        exploreCalendarsService.clearCalendar();
        exploreCalendarsService.setCalendar(calendarService.selectedCalendar);
        exploreCalendarsService.show();
        $rootScope.calendarModalDisplay = false;
        $rootScope.showExploreNotification = true;
    }

    $scope.dismiss = function () {
        exploreCalendarsService.clearCalendar();
        $rootScope.showExploreNotification = false;
    }
    
})