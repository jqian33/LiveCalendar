
myApp.directive('requiredTextInput', function() {
    return {
        require: "?ngModel",
        link: function (scope, element, attr, ngModel) {

            scope.$watch(function() { return ngModel.$modelValue }, function(newValue, oldValue) {
                if (oldValue !== "" && newValue === "") {
                    element.addClass("has-error");
                    scope.errorCount = scope.errorCount + 1;
                } else if ((oldValue === "" || oldValue === undefined) && (newValue !== "" && newValue !== undefined)) {
                    element.removeClass("has-error");
                    scope.errorCount = scope.errorCount - 1;
                }
            }, true);
        }
    }
})