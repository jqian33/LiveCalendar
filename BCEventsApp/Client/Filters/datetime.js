
myApp.filter("datetime", function($filter) {
    return function (input) {
        if (input == null) { return ""; }

        var date = $filter('date')(input, 'MMM dd yyyy');

        return date.toUpperCase();

    };
})
