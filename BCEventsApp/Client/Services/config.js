
myApp.factory('config', function() {
    var service = {};

    service.eventColors = {
        owned: "#28BEC5",
        subscribed: "#F7B733",
        explore: "#5BC0DE"
    }

    service.view = {
        calendarView: "#userCalendar"
    }

    service.searchIcons = {
        event: "glyphicon glyphicon-time",
        calendar: "glyphicon glyphicon-calendar"
    }

    service.checkBoxIcons = {
        checked: "glyphicon glyphicon-check",
        unchecked: "glyphicon glyphicon-unchecked"
    }

    return service;
})