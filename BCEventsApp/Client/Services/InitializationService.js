
myApp.factory('initializationService', function ($rootScope, searchService, timelineService, hubService, eventService, userCalendarsService, deinitializationService, subscribedEventsService, userService, config) {

    var service = {};

    service.initializeView = function () {
        $rootScope.navbarFirstname = "User";
        $rootScope.userCalendarView = {
            display: true
        }
        $rootScope.timelineView = {
            display: false
        }
        $rootScope.activeView = $rootScope.userCalendarView;
    }

    service.initialize = function (userId) {

        $rootScope.userId = userId;

        var token = localStorage.getItem("token");

        hubService.initializeHub();

        timelineService.retrieveTimeline(token)
            .then(function (response) {
                timelineService.setTimeline(response.data);
                $rootScope.timeline = timelineService.timeline;
                console.log(response.data);
            }, function(response) {
                if (response === 401) {
                    deinitializationService.appRefresh();
                }
            });

        searchService.retrieveSearchEvents(token)
            .then(function (response) {
                searchService.setSearchEvents(response.data);
                console.log(response.data);
            }, function (response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });

        searchService.retrieveSearchCalendars(token)
            .then(function(response) {
                searchService.setSearchCalendars(response.data);
                console.log(response.data);
            }, function(response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });

        userService.retrieveUser(token)
            .then(function (response) {
                userService.setUser(response.data);
                $rootScope.navbarFirstname = userService.user.FirstName;
            }, function (response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
        
        userCalendarsService.setUserId(userId);

        userCalendarsService.retrieveCalendars(token)
            .then(function (response) {
                console.log(response.data);
                userCalendarsService.setCalendars(response.data, userId);
                console.log(userCalendarsService.events);
                $rootScope.ownedCalendars = userCalendarsService.ownedCalendars;
                $rootScope.subscribedCalendars = userCalendarsService.subscribedCalendars;
                searchService.addUserPrivateEvents(userCalendarsService.events);
                searchService.addUserPrivateCalendars(userCalendarsService.calendars);

                subscribedEventsService.retrieveEvents(token)
                    .then(function (response) {
                        console.log(response.data);
                        subscribedEventsService.setEvents(response.data);
                        $rootScope.subscribedEvents = subscribedEventsService.displayEvents;
                    }, function (response) {
                        if (response.status === 401) {
                            deinitializationService.appRefresh();
                        }
                    });

                userCalendarsService.retrievePrimaryCalendarId(token)
                    .then(function (response) {
                        userCalendarsService.setPrimaryCalendarId(response.data);
                        console.log(response.data);
                        var primaryCalendarId = userCalendarsService.primaryCalendarId;
                        $rootScope.primaryCalendarId = primaryCalendarId;
                        var defaultCalendar = userCalendarsService.getCalendar(primaryCalendarId);
                        $(config.view.calendarView).fullCalendar('addEventSource', defaultCalendar);
                        userCalendarsService.setDisplay(primaryCalendarId, true);
                    }, function (response) {
                        if (response.status === 401) {
                            deinitializationService.appRefresh();
                        }
                    });
            }, function (response) {
                if (response.status === 401) {
                    deinitializationService.appRefresh();
                }
            });
    }
    
    return service;
})