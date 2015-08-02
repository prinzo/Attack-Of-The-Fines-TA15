app.directive('autoComplete', function(autoCompleteDataService) {
    return {
        restrict: 'A',
        link: function(scope, elem, attr, ctrl) {
                    // elem is a jquery lite object if jquery is not present,
                    // but with jquery and jquery ui, it will be a full jquery object.
            console.log("hitting directive");
            elem.autocomplete({
                source: ["Kristina Georgieva", "Prinay Panday", "Kurt Vining", "Amrit Purshotam"]
            });
        }
    };
});