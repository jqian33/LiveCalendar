
myApp.directive('eventDetailsModal', function($rootScope) {
    return {
        link: function(scope, element) {

            $("#deleteEventButton").click(function () {
                $("#deleteEventButton").attr('disabled', 'disabled');
            });

            $("#subscribeButton").click(function () {
                $("#subscribeButton").attr('disabled', 'disabled');
            });

            $("#unsubscribeButton").click(function () {
                $("#unsubscribeButton").attr('disabled', 'disabled');
            });

            element.on('hidden.bs.modal', function () {
                scope.attendButtonsDisplay = false;
                scope.numberOfAttendees = 0;
                $rootScope.eventModalDisplay = false;
                scope.showDeleteButton = false;
                scope.showSubscribeButton = false;
                scope.showUnsubscribeButton = false;
                scope.showEditButton = false;
                scope.eventTitle = "";
                scope.eventDescription = "";
                scope.eventLocation = "";
                scope.eventStartTime = null;
                scope.eventEndTime = null;
                scope.calendarTitle = "";
                $('#deleteEventButton').removeAttr('disabled');
                $('#subscribeButton').removeAttr('disabled');
                $('#unsubscribeButton').removeAttr('disabled');
                scope.$apply();
                $rootScope.$apply();
            });

            element.on('shown.bs.modal', function() {
                scope.initModal();
                scope.attendButtonsDisplay = true;
                scope.$apply();
                $rootScope.$apply();
            });

            scope.$watch(function () { return $rootScope.eventModalDisplay }, function (newValue, oldValue) {
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