
myApp.controller('navigationController', function($rootScope, $scope) {
    
    $scope.userCalendarView = function () {
        $rootScope.activeView.display = false;
        $rootScope.activeView = $rootScope.userCalendarView;
        $rootScope.activeView.display = true;

    }

    $scope.timelineView = function () {
        $rootScope.activeView.display = false;
        $rootScope.activeView = $rootScope.timelineView;
        $rootScope.activeView.display = true;
    }

    $scope.showSettings = function() {
        $rootScope.showSettings = true;
    }
})