
myApp.directive("settingsModal", function($rootScope) {
    return {
        link: function(scope, element) {

            element.on('hidden.bs.modal', function () {
                $rootScope.showSettings = false;
                $rootScope.$apply();
            });

            $rootScope.$watch(function () { return $rootScope.showSettings }, function (newValue, oldValue) {
                if (newValue === true) {
                    $("#starIcon").tooltip();
                    element.modal();
                }
            }, true);

        }
    }
})