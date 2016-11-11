
myApp.directive("userListPopover", function() {
    return {
        link: function(scope, element) {
            var options = {
                html: true,
                title: "They said they are going",
                content: function() {
                    return $("#myPopover").html();
                },
                placement: "left"
            }
            element.popover(options);

        }
    }
})