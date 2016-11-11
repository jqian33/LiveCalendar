myApp.controller("eventEditController", function ($scope, $rootScope, deinitializationService, basicService, timelineService, eventService, userService, eventEditService, userCalendarsService) {

    $scope.endTimeStatic = false;

    $scope.validationLookup = {
        startDate: true,
        startTime: true,
        endTime: true
    };

    $scope.initModal = function () {
        $scope.selectedDateTime = eventService.selectedEvent.start.local().toDate();
        $scope.startDate.setMoment(eventService.selectedEvent.start);
        $scope.oldStartDate = new Date(eventService.selectedEvent.start);
        $scope.endTime = eventService.selectedEvent.end.local().toDate();
        $scope.title = eventService.selectedEvent.title;
        $scope.description = eventService.selectedEvent.description;
        $scope.location = eventService.selectedEvent.location;
        if (userCalendarsService.getCalendar(eventService.selectedEvent.calendarId).isPublic === false) {
            $scope.showPublicSwitch = true;
        }
        $scope.isPublic = eventService.selectedEvent.isPublic;
    }

    $scope.modifyEvent = function () {
        
        var title = $scope.title;
        var description = $scope.description;
        var location = $scope.location;
        var duration = Math.abs($scope.endTime.getTime() - $scope.selectedDateTime.getTime()) / (60 * 1000);
        var eventObject = {
            Id: eventService.selectedEvent.id,
            Title: title,
            Description: description,
            Location: location,
            Start: $scope.selectedDateTime,
            End: null,
            Duration: duration,
            RRule: eventService.selectedEvent.rRule,
            Tags: [],
            Owner: eventService.selectedEvent.owner,
            Attendees: eventService.selectedEvent.attendees,
            CalendarId: eventService.selectedEvent.calendarId,
            Public: $scope.isPublic
        }

        var startDateChanged = false;
        if (moment(eventObject.Start).startOf('day') !== moment(eventService.selectedEvent.start).startOf('day')) {
            startDateChanged = true;
        }

        var token = localStorage.getItem("token");
        eventEditService.editRequest(token, eventObject)
            .then(function() {
                userCalendarsService.updateEvent(eventObject);
                var ownerName = eventObject.Owner.FirstName + " " + eventObject.Owner.LastName;
                var timelineEvent = {
                    Id: eventObject.Id,
                    Title: eventObject.Title,
                    OwnerName: ownerName,
                    Start: eventObject.Start,
                    AttendeesCount: eventObject.Attendees.length
                }
                if (eventService.selectedEvent.isPublic === true && eventObject.Public === true) {
                    $.connection.eventHub.server.editEvent(eventObject.Id);
                    if (startDateChanged === true) {
                        timelineService.deleteEntry(eventObject.Id, $scope.oldStartDate);
                        timelineService.addEntry(timelineEvent);
                        $.connection.eventHub.server.changeStartDate($scope.oldStartDate, eventObject.Id);
                    }
                }
                else if (eventService.selectedEvent.isPublic === false && eventObject.Public === true) {
                    var searchEvent = {
                        Id: eventObject.Id,
                        Title: eventObject.Title,
                        OwnerName: ownerName
                    }
                    timelineService.addEntry(timelineEvent);
                    $.connection.eventHub.server.addEvent(searchEvent, eventObject.CalendarId);
                } else {
                    timelineService.deleteEntry(eventObject.Id, eventObject.Start);
                    $.connection.eventHub.server.deleteEvent(eventObject.Id, eventObject.CalendarId, eventObject.Start);
                }
            }, function (response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });

        $rootScope.showEventEdit = false;

    }
})