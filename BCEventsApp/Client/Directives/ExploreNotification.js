
myApp.directive("exploreNotification", function($rootScope) {
    return {
        link: function(scope, element) {
            
            $('#exploreNotification').hide();

            $rootScope.$watch(function () { return $rootScope.showExploreNotification }, function (newValue) {
                if (newValue === true) {
                    $('#exploreNotification').show();
                }
                else if (newValue === false) {
                    $('#exploreNotification').hide();
                }
            }, true);
        }
    }
});