
myApp.directive("calendarDetailsModal", function($rootScope) {
    return {
        link: function(scope, element) {
            
            element.find("#exploreButton").click(function () {
                element.find("#exploreButton").attr('disabled', 'disabled');
            });

            element.on('hidden.bs.modal', function () {
                $rootScope.calendarModalDisplay = false;
                scope.calendarTitle = "";
                scope.calendarDescription = "";
                scope.calendarOwner = "";
                scope.eventCount = "";
                scope.showExploreButton = false;
                $('#exploreButton').removeAttr('disabled');
                scope.$apply();
                $rootScope.$apply();
            });

            element.on('shown.bs.modal', function() {
                $rootScope.calendarModalDisplay = true;
                scope.initModal();
                scope.$apply();
                $rootScope.$apply();
            });

            scope.$watch(function () { return $rootScope.calendarModalDisplay }, function (newValue, oldValue) {
                if (newValue === true) {
                    element.modal('show');
                }
                if (oldValue === true && newValue === false) {
                    element.modal('hide');
                }
            }, true);
        }
    }
})