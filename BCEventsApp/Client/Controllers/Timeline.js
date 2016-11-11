
myApp.controller("timelineController", function ($scope, $rootScope, timelineService, userCalendarsService, subscribedEventsService, eventService, deinitializationService) {

    $scope.weekDayColor = function(input) {
        if (input == null) {
            return "";
        }

        var date = moment(input);
        switch (date.day()) {
            case 0:
                return "sunday";
            case 1:
                return "monday";
            case 2:
                return "tuesday";
            case 3:
                return "wednesday";
            case 4:
                return "thursday";
            case 5:
                return "friday";
            case 6:
                return "saturday";
        }
        return "";
    }

    $scope.showEventDetails = function (eventId) {
        var event = userCalendarsService.getEventById(eventId);
        if (event != null) {
            eventService.setSelectedEvent(event);
        } else {
            event = subscribedEventsService.getEventById(eventId);
            if (event != null) {
                eventService.setSelectedEvent(event);
            } else {
                var token = localStorage.getItem("token");
                eventService.retrieveEventById(token, eventId).
                    then(function (response) {
                        eventService.setServerEvent(response.data);
                    }, function (response) {
                        if (response.status === 401) {
                            deinitializationService.appRefresh();
                        }
                    });
            }
        }
        $rootScope.eventModalDisplay = true;
    }

    $scope.loadMore = function () {
        console.log("loading more...");
        var timeline = $rootScope.timeline;
        var length = timeline.length;
        var lastEntry = timeline[length - 1].Date;
        var token = localStorage.getItem("token");
        timelineService.retrieveMoreEntries(token, lastEntry)
            .then(function(response) {
                if (response.data != null) {
                    var data = response.data;
                    for (var i = 0; i < data.length; i++) {
                        $rootScope.timeline.push(response.data[i]);
                    }
                }
            }, function(response) {
                if (response === 401) {
                    deinitializationService.appRefresh();
                }
            });
    }
})