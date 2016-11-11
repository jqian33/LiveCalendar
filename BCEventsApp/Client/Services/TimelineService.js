
myApp.factory("timelineService", function ($http, utilities) {

    var service = {};

    service.timeline = [];

    service.setTimeline = function(data) {
        service.timeline = data;
    }

    service.getTimelineHead = function() {
        return moment(service.timeline[0].Date);
    }

    service.getTimelineTail = function() {
        return moment(service.timeline[service.timeline.length - 1].Date);
    }

    service.addEntry = function (entry) {
        var date = moment(entry.Start).startOf('day');
        var location = utilities.indexOfDate(date, service.timeline, "Date");
        if (location !== -1) {
            utilities.insertSortedDecending(service.timeline[location].EventList, entry, "AttendeesCount");
        } else {
            var head = service.getTimelineHead();
            var tail = service.getTimelineTail();
            var range = moment().range(head, tail);
            if (range.contains(date)) {
                var dateString = date.format("YYYY-MM-DDT00:00:00");
                var newEntry = {
                    Date: dateString,
                    EventList: []
                }
                newEntry.EventList.push(entry);
                utilities.insertSortedDate(service.timeline, newEntry, "Date");
            }
        }
    }

    service.deleteEntry = function(eventId, eventStart) {
        var date = moment(eventStart).startOf('day');
        var location = utilities.indexOfDate(date, service.timeline, "Date");
        if (location !== -1) {
            var eventList = service.timeline[location].EventList;
            for (var i = 0; i < eventList.length; i++) {
                if (eventList[i].Id === eventId) {
                    eventList.splice(i, 1);
                    break;
                }
            }
            if (service.timeline[location].EventList.length === 0) {
                service.timeline.splice(location, 1);
            }
        }
    }

    service.eventAddedAttendee = function(eventId, eventStart) {
        var event = null;
        var date = moment(eventStart).startOf('day');
        var location = utilities.indexOfDate(date, service.timeline, "Date");
        if (location !== -1) {
            var eventList = service.timeline[location].EventList;
            for (var i = 0; i < eventList.length; i++) {
                if (eventList[i].Id === eventId) {
                    event = eventList[i];
                    eventList.splice(i, 1);
                    break;
                }
            }
        }
        if (event !== null && event !== undefined) {
            event.AttendeesCount++;
            utilities.insertSortedDecending(service.timeline[location].EventList, event, "AttendeesCount");
        }
    }

    service.eventRemovedAttendee = function (eventId, eventStart) {
        var event = null;
        var date = moment(eventStart).startOf('day');
        var location = utilities.indexOfDate(date, service.timeline, "Date");
        if (location !== -1) {
            var eventList = service.timeline[location].EventList;
            for (var i = 0; i < eventList.length; i++) {
                if (eventList[i].Id === eventId) {
                    event = eventList[i];
                    eventList.splice(i, 1);
                    break;
                }
            }
        }
        if (event !== null && event !== undefined) {
            event.AttendeesCount--;
            utilities.insertSortedDecending(service.timeline[location].EventList, event, "AttendeesCount");
        }
    }

    service.retrieveTimeline = function(token) {
        return $http({
            method: "POST",
            url: "api/Global/GetTimeline",
            params: { token: token },
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.retrieveMoreEntries = function(token, lastEntryDate) {
        return $http({
            method: "POST",
            url: "api/Global/GetMoreTimelineEntries",
            params: { token: token },
            data: JSON.stringify(lastEntryDate),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    service.retrieveTimelineEvent = function (token, eventId) {
        return $http({
            method: "POST",
            url: "api/Global/GetTimelineEvent",
            params: { token: token },
            data: JSON.stringify(eventId),
            headers: { 'Content-Type': 'application/json' }
        });
    }

    return service;
})