
myApp.directive('confirmButton', function() {
    return {
        link: function(scope, element) {
            scope.$watch(function () { return scope.editing }, function (newValue) {
                if (newValue === true) {
                    element.attr('disabled', 'disabled');
                }
                else if ((newValue === false) && scope.errorCount === 0) {
                    element.removeAttr('disabled');
                }
            }, true);

            scope.$watch(function () { return scope.errorCount }, function () {
                var errorCount = scope.errorCount;
                if (scope.errorCount === 0) {
                    element.removeAttr('disabled');
                } else {
                    element.attr('disabled', 'disabled');
                }
            }, true);
        }
    }
})