myApp.directive("addCalendarButton", function($rootScope) {
    return {
        link: function(scope, element) {

            element.on("click", function () {
                $rootScope.showCalendarCreation = true;
                $rootScope.$apply();
            });
        }
    }
})