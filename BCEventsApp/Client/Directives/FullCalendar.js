
myApp.directive('fullCalendarUi', function ($rootScope, userCalendarsService, eventService, subscribedEventsService, exploreCalendarsService) {
    return {
        link: function (scope, element) {

            var options = {
                fixedWeekCount: false,
                height: 750,
                header: {
                    left: 'prev,next today',
                    center: 'title',
                    right: 'month,agendaWeek,agendaDay'
                },
                eventClick: function (event, jsEvent, view) {
                    eventService.setSelectedEvent(event);
                    $rootScope.eventModalDisplay = true;
                    $rootScope.$apply();
                },
                eventRender: function (event, element) {
                    element.css('cursor', 'pointer');
                },
                dayClick: function (date) {
                    userCalendarsService.setSelectedDate(date);
                    $('#eventCreationModal').modal();
                }
            };

            var resolveDuplication = function () {
                var refreshSubscribedEvents = false;
                if (subscribedEventsService.displayEvents.display === true) {
                    for (var key in subscribedEventsService.events) {
                        if (subscribedEventsService.events.hasOwnProperty(key)) {
                            if (exploreCalendarsService.events.hasOwnProperty(key)) {
                                element.fullCalendar('removeEvents', exploreCalendarsService.events[key].id);
                                refreshSubscribedEvents = true;
                            }
                        }
                    }
                }
                return refreshSubscribedEvents;
            }

            var refreshEventSource = function(eventSource) {
                element.fullCalendar('removeEventSource', eventSource);
                element.fullCalendar('addEventSource', eventSource);
            }

            element.fullCalendar(options);

            // Rendar calendar view
            $rootScope.$watch(function () { return $rootScope.userCalendarView.display }, function (newValue) {
                console.log(newValue);
                if (newValue === true) {
                    element.show();
                    for (var key in userCalendarsService.calendars) {
                        if (userCalendarsService.calendars.hasOwnProperty(key)) {
                            var calendar = userCalendarsService.calendars[key];
                            if (calendar.display === true) {
                                refreshEventSource(calendar);
                            } else {
                                element.fullCalendar('removeEventSource', calendar);
                            }
                        }
                    }
                    
                    var subscribedEvents = subscribedEventsService.displayEvents;
                    if (subscribedEvents.display === true) {
                        refreshEventSource(subscribedEvents);
                    } else {
                        element.fullCalendar('removeEventSource', subscribedEvents);
                    }

                    var exploreCalendar = exploreCalendarsService.calendar;
                    if (exploreCalendar !== null) {
                        refreshEventSource(exploreCalendar);
                    }

                }
                else if (newValue === false) {
                    element.hide();
                }
            }, true);

            // Watch for event source changes in userCalendarsService
            $rootScope.$watch(function () { return userCalendarsService.addedEventSource }, function (newValue) {
                if (newValue !== undefined ) {
                    element.fullCalendar('addEventSource', newValue);
                    userCalendarsService.resetAddedEventSource();
                }
            }, true);

            $rootScope.$watch(function() { return userCalendarsService.removedEventSource }, function(newValue) {
                if (newValue !== undefined) {
                    element.fullCalendar('removeEventSource', newValue);
                    userCalendarsService.resetRemovedEventSource();
                }
            }, true);

            $rootScope.$watch(function() { return userCalendarsService.refreshedEventSource }, function(newValue) {
                if (newValue !== undefined) {
                    refreshEventSource(newValue);
                    userCalendarsService.resetRefreshedEventSource();
                }
            }, true);

            // Watch for event source changes in subscribedEventsService 
            $rootScope.$watch(function () { return subscribedEventsService.addedEventSource }, function (newValue) {
                if (newValue !== undefined) {
                    element.fullCalendar('addEventSource', newValue);
                    if (resolveDuplication() === true) {
                        refreshEventSource(subscribedEventsService.displayEvents);
                    }
                    subscribedEventsService.resetAddedEventSource();
                }
            }, true);

            $rootScope.$watch(function () { return subscribedEventsService.removedEventSource }, function (newValue) {
                if (newValue !== undefined) {
                    element.fullCalendar('removeEventSource', newValue);
                    subscribedEventsService.resetRemovedEventSource();
                }
            }, true);

            $rootScope.$watch(function () { return subscribedEventsService.refreshedEventSource }, function (newValue) {
                if (newValue !== undefined) {
                    if (exploreCalendarsService.calendar !== null) {
                        refreshEventSource(exploreCalendarsService.calendar);
                    }
                    resolveDuplication();
                    refreshEventSource(subscribedEventsService.displayEvents);
                    subscribedEventsService.resetRefreshedEventSource();
                }
            }, true);


            // Watch for event source changes in exploreCalendarsService
            $rootScope.$watch(function () { return exploreCalendarsService.addedEventSource }, function (newValue) {
                if (newValue !== undefined) {
                    element.fullCalendar('addEventSource', newValue);
                    if (resolveDuplication() === true) {
                        refreshEventSource(subscribedEventsService.displayEvents);
                    }
                    exploreCalendarsService.resetAddedEventSource();
                }
            }, true);

            $rootScope.$watch(function () { return exploreCalendarsService.removedEventSource }, function (newValue) {
                if (newValue !== undefined) {
                    element.fullCalendar('removeEventSource', newValue);
                    exploreCalendarsService.resetRemovedEventSource();
                }
            }, true);

            $rootScope.$watch(function () { return exploreCalendarsService.refreshedEventSource }, function (newValue) {
                if (newValue !== undefined) {
                    refreshEventSource(newValue);
                    if (resolveDuplication() === true) {
                        refreshEventSource(subscribedEventsService.displayEvents);
                    }
                    exploreCalendarsService.resetRefreshedEventSource();
                }
            }, true);

        }
    }
})