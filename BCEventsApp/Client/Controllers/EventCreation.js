
myApp.controller("EventCreationController", function ($scope, deinitializationService, timelineService, userCalendarsService, eventCreationService, userService, basicService, searchService) {

    $scope.endTimeStatic = true;

    $scope.validationLookup = {
        startDate: true,
        startTime: true,
        endTime: true
    };

    $scope.calculateEndTime = function(startTime) {
        var endTime = new Date(startTime.getTime());
        endTime.setMinutes(endTime.getMinutes() + 60);
        return endTime;
    };

    $scope.calendarSelect = function(calendar) {
        $scope.calendarSelectionTitle = calendar.title;
        $scope.calendarSelectionId = calendar.id;
        if (calendar.isPublic === true) {
            $scope.showPublicCheckBox = false;
            $scope.publish = true;
        } else {
            $scope.showPublicCheckBox = true;
            $scope.publish = false;
        }
    }

    $scope.initModal = function () {
        $scope.modalDisplay = true;
        var input = userCalendarsService.selectedDate;
        $scope.selectedDateTime = moment(input.format()).toDate();
        $scope.startDate.setMoment(moment(input.format()));
        $scope.endTime = $scope.calculateEndTime($scope.selectedDateTime);
        $scope.ownedCalendars = userCalendarsService.ownedCalendars;
        $scope.calendarSelectionTitle = userCalendarsService.calendars[userCalendarsService.primaryCalendarId].title;
        $scope.calendarSelectionId = userCalendarsService.primaryCalendarId;
        if (userCalendarsService.calendars[userCalendarsService.primaryCalendarId].isPublic === true) {
            $scope.showPublicCheckBox = false;
            $scope.publish = true;
        } else {
            $scope.showPublicCheckBox = true;
            $scope.publish = false;
        }
    }

    $scope.createEvent = function () {

        var title = $scope.title;
        var description = $scope.description;
        var location = $scope.location;
        var duration = Math.abs($scope.endTime.getTime() - $scope.selectedDateTime.getTime()) / (60 * 1000);

        var eventObject = {
            Id: -1,
            Title: title,
            Description: description,
            Location: location,
            Start: $scope.selectedDateTime,
            End: null,
            Duration: duration,
            RRule: null,
            Tags: [],
            Owner: null,
            Attendees: [],
            CalendarId: $scope.calendarSelectionId,
            Public: $scope.publish
        };
        console.log(eventObject);

        var token = localStorage.getItem("token");
        eventCreationService.createEvent(token, eventObject).
            then(function (response) {
                console.log(response);
                if (response.status === 200) {
                    eventObject.Id = response.data;
                    eventObject.Owner = userService.user;
                    
                    userCalendarsService.addEvent(eventObject, $scope.calendarSelectionId);

                    var ownerName = eventObject.Owner.FirstName + " " + eventObject.Owner.LastName;
                    var searchEvent = {
                        Id: eventObject.Id,
                        Title: eventObject.Title,
                        OwnerName: ownerName
                    }
                    if (eventObject.Public === true) {
                        var timelineEvent = {
                            Id: eventObject.Id,
                            Title: eventObject.Title,
                            OwnerName: ownerName,
                            Start: eventObject.Start,
                            AttendeesCount: 0
                        }
                        timelineService.addEntry(timelineEvent);
                        $.connection.eventHub.server.addEvent(searchEvent, eventObject.CalendarId);
                    }
                    searchService.addSearchEvent(searchEvent);
                }
            }, function (response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });

        $scope.modalDisplay = false;
    }

});