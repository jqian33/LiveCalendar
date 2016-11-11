
myApp.factory('basicService', function() {

    var service = {};

    service.serverToUiEvent = function(serverEvent) {
        var endTime = moment(serverEvent.Start).add(serverEvent.Duration, 'minutes');
        var event = {
            id: serverEvent.Id,
            title: serverEvent.Title,
            description: serverEvent.Description,
            location: serverEvent.Location,
            start: moment(serverEvent.Start),
            end: endTime,
            duration: serverEvent.Duration,
            owner: serverEvent.Owner,
            tags: serverEvent.Tags,
            isPublic: serverEvent.Public,
            rRule: serverEvent.RRule,
            calendarId: serverEvent.CalendarId,
            attendees: serverEvent.Attendees,
            exploreEvent: false,
            uiEvent: true
        }
        return event;
    }

    service.getRecurrenceInstance = function(repeats, event, index) {
        var timeZoneOffSet = repeats[index].getTimezoneOffset() * -1;
        var instanceStartTime = new Date(repeats[index].toISOString());
        var instanceEndTime = new Date(repeats[index].toISOString());
        instanceStartTime.setMinutes(instanceStartTime.getMinutes() + timeZoneOffSet);
        instanceEndTime.setMinutes(instanceEndTime.getMinutes() + timeZoneOffSet + event.Duration);
        var eventInstance = {
            id: event.Id,
            title: event.Title,
            description: event.Description,
            location: event.Location,
            start: instanceStartTime.toISOString(),
            end: instanceEndTime.toISOString(),
            duration: event.Duration,
            owner: event.Owner,
            tags: event.Tags,
            isPublic: event.Public,
            rRule: event.RRule,
            calendarId: event.CalendarId,
            attendees: event.Attendees,
            exploreEvent: false,
            uiEvent: false
        };
        return eventInstance;
    }

    service.getRecurrenceInstanceFromDisplayEvent = function (repeats, event, index) {
        var timeZoneOffSet = repeats[index].getTimezoneOffset() * -1;
        var instanceStartTime = new Date(repeats[index].toISOString());
        var instanceEndTime = new Date(repeats[index].toISOString());
        instanceStartTime.setMinutes(instanceStartTime.getMinutes() + timeZoneOffSet);
        instanceEndTime.setMinutes(instanceEndTime.getMinutes() + timeZoneOffSet + event.duration);
        var eventInstance = {
            id: event.id,
            title: event.title,
            description: event.description,
            location: event.location,
            start: instanceStartTime.toISOString(),
            end: instanceEndTime.toISOString(),
            duration: event.duration,
            owner: event.owner,
            tags: event.tags,
            isPublic: event.isPublic,
            rRule: event.rRule,
            calendarId: event.calendarId,
            attendees: event.attendees,
            exploreEvent: false,
            uiEvent: false
        };
        return eventInstance;
    }

    service.uiToDisplayEvent = function(event) {
        var displayEvent = {
            id: event.id,
            title: event.title,
            description: event.description,
            location: event.location,
            start: event.start.toDate(),
            end: event.end.toDate(),
            duration: event.duration,
            owner: event.owner,
            tags: event.tags,
            isPublic: event.isPublic,
            rRule: event.rRule,
            calendarId: event.calendarId,
            attendees: event.attendees,
            exploreEvent: false,
            uiEvent: false
        }
        return displayEvent;
    }

    service.serverToDisplayEvent = function(event) {
        var displayEvent = {
            id: event.Id,
            title: event.Title,
            description: event.Description,
            location: event.Location,
            start: event.Start,
            end: moment(event.Start).add(event.Duration, 'minutes').toDate(),
            duration: event.duration,
            tags: event.Tags,
            owner: event.Owner,
            isPublic: event.Public,
            rRule: event.RRule,
            calendarId: event.CalendarId,
            attendees: event.Attendees,
            exploreEvent: false,
            uiEvent: false
        }
        return displayEvent;
    }

    service.exploreToSubscribedDisplayEvent = function(event) {
        var subscribedDisplayEvent = {
            id: event.id,
            title: event.title,
            description: event.description,
            location: event.location,
            start: event.start,
            end: event.end,
            duration: event.duration,
            tags: event.tags,
            owner: event.owner,
            isPublic: event.isPublic,
            rRule: event.rRule,
            calendarId: event.calendarId,
            attendees: event.attendees,
            exploreEvent: false,
            uiEvent: false
        }
        return subscribedDisplayEvent;
    }

    return service;
})
