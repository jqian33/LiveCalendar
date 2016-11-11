
myApp.filter("time", function () {
    return function (input) {

        if (input == null) {
            return "";
        }

        var date = moment(input);
        return date.format("LT");
    };
})