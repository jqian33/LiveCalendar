
myApp.directive('globalSearch', function ($rootScope, searchService, eventService, userCalendarsService, deinitializationService, subscribedEventsService, calendarService, exploreCalendarsService, config) {
    return {
        link: function (scope, element) {
            var eventMatcher = function () {
                return function findMatches(q, cb) {

                    var events = searchService.searchEvents;

                    var calendars = searchService.searchCalendars;

                    // an array that will be populated with substring matches
                    var matches = [];

                    // regex used to determine if a string contains the substring `q`
                    var substrRegex = new RegExp(q, 'i');

                    // iterate through the pool of strings and for any string that
                    // contains the substring `q`, add it to the `matches` array
                    for (var i = 0; i < events.length; i++) {
                        if (substrRegex.test(events[i].Title) === true || substrRegex.test(events[i].OwnerName) === true) {
                            events[i].IsEvent = true;
                            events[i].IsCalendar = false;
                            matches.push(events[i]);
                        }
                    }
                    for (var j = 0; j < calendars.length; j++) {
                        if (substrRegex.test(calendars[j].Title) === true || substrRegex.test(calendars[j].OwnerName) === true) {
                            calendars[j].IsEvent = false;
                            calendars[j].IsCalendar = true;
                            matches.push(calendars[j]);
                        }
                    }

                    cb(matches);
                };
            };

            element.typeahead({
                hint: true,
                highlight: true,
                minLength: 1
            },
            {
                source: eventMatcher(),
                display: 'Title',
                templates: {
                    suggestion: function (data) {
                        if (data.IsEvent === true) {
                            return '<p class="wordWrap"><i class="' + config.searchIcons.event + '"></i><strong>' + "\t" + data.Title + '</strong> - ' + data.OwnerName + '</p>';

                        } else if (data.IsCalendar === true) {
                            return '<p class="wordWrap"><i class="' + config.searchIcons.calendar + '"></i><strong>' + "\t" + data.Title + '</strong> - ' + data.OwnerName + '</p>';
                        }
                    }
                }
            });

            element.bind('typeahead:select', function (ev, suggestion) {
                console.log('Selection: ' + suggestion.Id);
                var token = localStorage.getItem("token");
                if (suggestion.IsEvent === true) {
                    var event = userCalendarsService.getEventById(suggestion.Id);
                    if (event != null) {
                        eventService.setSelectedEvent(event);
                    } else {
                        event = subscribedEventsService.getEventById(suggestion.Id);
                        if (event != null) {
                            eventService.setSelectedEvent(event);
                        } else {
                            eventService.retrieveEventById(token, suggestion.Id).
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
                } else if (suggestion.IsCalendar === true) {
                    var cachedExploreCalendar = exploreCalendarsService.getCachedCalendar(suggestion.Id);
                    if (userCalendarsService.serverCalendars.hasOwnProperty(suggestion.Id)) {
                        calendarService.setSelectedCalendar(userCalendarsService.serverCalendars[suggestion.Id]);
                    }
                    else if (cachedExploreCalendar !== null) {
                        calendarService.setSelectedCalendar(cachedExploreCalendar);
                    } else {
                        calendarService.retrieveCalendarById(token, suggestion.Id)
                        .then(function (response) {
                            calendarService.setSelectedCalendar(response.data);
                        }, function (response) {
                            if (response.status === 401) {
                                deinitializationService.appRefresh();
                            }
                        });
                    }
                    $rootScope.calendarModalDisplay = true;
                }
                $rootScope.$apply();
            });
        }
    }
})