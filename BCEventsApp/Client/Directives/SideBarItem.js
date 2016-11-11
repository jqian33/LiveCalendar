
myApp.directive("sideBarItem", function(config) {
    return {
        require: "?ngModel",
        link: function (scope, element, attribute, ngModel) {

            scope.$watch(function () { return ngModel.$modelValue}, function (newValue) {
                if (newValue !== undefined && newValue !== null) {
                    if (newValue.display === true) {
                        element.find("span").removeClass(config.checkBoxIcons.unchecked);
                        element.find("span").addClass(config.checkBoxIcons.checked);
                    } else if (newValue.display === false) {
                        element.find("span").removeClass(config.checkBoxIcons.checked);
                        element.find("span").addClass(config.checkBoxIcons.unchecked);
                    }
                }
            }, true);
        }
    }
})